using Blog_Server.Interfaces.Services;
using Blog_Server.Models.AuthModels;
using Blog_Server.Models.JwtModels;
using Blog_Server.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog_Server.Models.ResponseModels;
using Blog_Server.Interfaces.UnitOfWork;
using Blog_Server.Database.Entities;
using Blog_Server.Exceptions;

namespace Blog_Server.Services
{
    public class AuthService : IAuthService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtOptions _jwtOptions;

        private SigningCredentials issuerSigningCredentials;
        #endregion

        #region Constructors
        public AuthService(IUnitOfWork unitOfWork, JwtOptions jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _jwtOptions = jwtOptions;

            issuerSigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)), SecurityAlgorithms.HmacSha256);
        }
        #endregion

        #region Public methods
        public async Task<BaseResponseModel> GetTokenAsync(AuthRequestModel requestModel)
        {
            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(requestModel.Login);
            
            if (user is null || !CheckUserPassword(requestModel.Password, user))
            {
                throw new AppException("Invalid login or password");
            }

            var claims = new List<Claim> { new Claim(
                ClaimTypes.NameIdentifier, user.Login) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;
            var jwtToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer, audience: _jwtOptions.Audience, notBefore: now,
                claims: claimsIdentity.Claims, expires: now.AddSeconds(_jwtOptions.ExpirationSeconds),
                signingCredentials: issuerSigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);


            return new BaseResponseModel() { Data = new AuthResponseDataModel { AccessToken = encodedJwt } };
        }

        public async Task<BaseResponseModel> RegisterNewUserAsync(AuthSignUpRequestModel requestModel)
        {
            var response = new BaseResponseModel();

            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(requestModel.Login);
            if (user is not null)
            {
                throw new AppException("Same login already exists");
            }

            user = await _unitOfWork.UsersRepository.GetUserByEmailAsync(requestModel.Email);
            if (user is not null)
            {
                throw new AppException("Email already taken");
            }

            user = new User
            {
                Email = requestModel.Email,
                Login = requestModel.Login,
                PasswordHash = HashHelper.getSHA256(requestModel.Password)
            };

            try
            {
                await _unitOfWork.UsersRepository.InsertAsync(user);
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                throw new AppException("User saving error");
            }

            return response;
        }
        #endregion

        #region Private mewthods
        private bool CheckUserPassword(string password, User user)
        {
            if (user.PasswordHash != HashHelper.getSHA256(password))
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
