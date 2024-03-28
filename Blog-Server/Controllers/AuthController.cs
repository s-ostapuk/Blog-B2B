using Blog_Server.Interfaces.Services;
using Blog_Server.Models.AuthModels;
using Blog_Server.Models.JwtModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Properties
        private readonly IAuthService _authService;
        #endregion

        #region Constructors
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Controllers
        [HttpPost]
        public async Task<AuthResponseModel> GetTokenAsync([FromBody] AuthRequestModel requestModel)
        {
            return await _authService.GetTokenAsync(requestModel);
        }
        #endregion
    }
}
