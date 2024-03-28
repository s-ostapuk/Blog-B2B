using Blog_Server.Interfaces.Services;
using Blog_Server.Interfaces.UnitOfWork;
using Blog_Server.Models.AuthModels;
using Blog_Server.Models.Database.Entities;
using Blog_Server.Models.JwtModels;
using Blog_Server.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog_Server.Services
{
    public class AuthService : IAuthService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtOptions _jwtOptions;

        private TimeSpan tokenLifetimeMinutes = TimeSpan.FromMinutes(15);
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
        public async Task<AuthResponseModel> GetTokenAsync(AuthRequestModel requestModel)
        {
            if (requestModel == null || string.IsNullOrEmpty(requestModel.Login) || string.IsNullOrEmpty(requestModel.Password))
            {
                var response = new AuthResponseModel();
                response.Errors.Add("Id and Secret is required");
                return response;
            }

            var user = await _unitOfWork.UsersRepository.GetUserByLoginAsync(requestModel.Login);
            if (user == null)
            {
                var response = new AuthResponseModel();
                response.Errors.Add("User not found");
                return response;
            }

            if (!CheckUserPassword(requestModel.Password, user))
            {
                var response = new AuthResponseModel();
                response.Errors.Add("Password is invalid");
                return response;
            }

            var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;
            var jwtToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer, audience: _jwtOptions.Audience, notBefore: now,
                claims: claimsIdentity.Claims, expires: now.Add(tokenLifetimeMinutes),
                signingCredentials: issuerSigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new AuthResponseModel() { AccessToken = encodedJwt };
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
