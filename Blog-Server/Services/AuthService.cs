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
            var response = new BaseResponseModel();

            if (requestModel == null || string.IsNullOrEmpty(requestModel.Login) || string.IsNullOrEmpty(requestModel.Password))
            {
                response.Errors.Add("Login and Password is required");
                return response;
            }

            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(requestModel.Login);
            if (user == null)
            {
                response.Errors.Add("User not found");
                return response;
            }

            if (!CheckUserPassword(requestModel.Password, user))
            {
                response.Errors.Add("Password is invalid");
                return response;
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

            if (requestModel is null || string.IsNullOrEmpty(requestModel.Login) || string.IsNullOrEmpty(requestModel.Password))
            {
                response.Errors.Add("Login and Password is required");
                return response;
            }

            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(requestModel.Login);
            if (user != null)
            {
                response.Errors.Add("User already exists");
                return response;
            }

            user = await _unitOfWork.UsersRepository.GetUserByEmailAsync(requestModel.Email);
            if (user != null)
            {
                response.Errors.Add("Email already taken");
                return response;
            }

            user = new User
            {
                Email = requestModel.Email,
                Login = requestModel.Login,
                PasswordHash = HashHelper.getSHA256(requestModel.Password)
            };

            var newUser = await _unitOfWork.UsersRepository.InsertAsync(user);

            if (newUser is null)
            {
                response.Errors.Add("User creation error");
                return response;
            }

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch
            {
                response.Errors.Add("User saving error");
                return response;
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
