using Blog_Server.Controllers;
using Blog_Server.Interfaces.Services;
using Blog_Server.Models.AuthModels;
using Blog_Server.Models.JwtModels;
using Blog_Server.Models.ResponseModels;
using Blog_Server.Services;
using BlogTests.MockData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BlogTests
{
    public class AuthControllerTests
    {
        private readonly AuthController _authController;
        private readonly Mock<IAuthService> _authService;
        private readonly string tokenSigningKey = "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTcxMTYxNTc2NCwiaWF0IjoxNzExNjE1NzY0fQ.-PhsZt8Mk58zbiJf-0OTP1lytgdwiaTC2sPeqJts_qY";

        public AuthControllerTests()
        {
            _authService = new Mock<IAuthService>();
            _authController = new AuthController(_authService.Object);
        }

        [Fact]
        public async Task GetToken_WithExistingCredentialsSuccess()
        {
            var mockRequestModel = new AuthRequestModel
            {
                Login = "TestUser1",
                Password = "TestUser1",
            };
            //Arrange
            _authService.Setup(service => service.GetTokenAsync(mockRequestModel)).ReturnsAsync(AuthMockData.GetExistingCredentialsTokenData());
            //Act
            var result = await _authController.GetToken(mockRequestModel);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BaseResponseModel>(result);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.True(result.IsSuccess);
            Assert.IsType<AuthResponseDataModel>(result.Data);
            var userNameFromTokenClaims = ValidateToken(((AuthResponseDataModel)result.Data).AccessToken)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Assert.Equal(mockRequestModel.Login, userNameFromTokenClaims);
        }
        [Fact]
        public async Task GetToken_WithInvalidCredentialsSuccess()
        {
            var mockRequestModel = new AuthRequestModel
            {
                Login = "TestUser",
                Password = "TestUser",
            };
            //Arrange
            _authService.Setup(service => service.GetTokenAsync(mockRequestModel)).ReturnsAsync(AuthMockData.GetInvalidCredentialsTokenData());
            //Act
            var result = await _authController.GetToken(mockRequestModel);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BaseResponseModel>(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
        }
        [Fact]
        public async Task RegisterNewUser_Success()
        {
            var mockRequestModel = new AuthSignUpRequestModel
            {
                Login = "TestUser1",
                Password = "TestUser1",
                Email = "TestUser1@gmail.com"
            };
            //Arrange
            _authService.Setup(service => service.RegisterNewUserAsync(mockRequestModel)).ReturnsAsync(AuthMockData.GetRegisterNewUserSuccessResponse());
            //Act
            var result = await _authController.RegisterNewUser(mockRequestModel);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<BaseResponseModel>(result);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.True(result.IsSuccess);
            Assert.Null(result.Data);
        }
        [Fact]
        public void GetToken_ActionDoNotRequiresAuthorization()
        {
            // Arrange
            var type = _authController.GetType();
            var methodInfo = type.GetMethod("GetToken");
            // Act
            var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            // Assert
            Assert.False(attributes.Any(), "No AuthorizeAttribute found on action method.");
        }
        [Fact]
        public void RegisterNewUser_ActionDoNotRequiresAuthorization()
        {
            // Arrange
            var type = _authController.GetType();
            var methodInfo = type.GetMethod("RegisterNewUser");
            // Act
            var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            // Assert
            Assert.False(attributes.Any(), "No AuthorizeAttribute found on action method.");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = "https://localhost:7299".ToLower();
            validationParameters.ValidIssuer = "https://localhost:7299".ToLower();
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSigningKey));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }
    }
}