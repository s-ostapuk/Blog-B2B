using Blog_Server.Interfaces.Services;
using Blog_Server.Models.AuthModels;
using Blog_Server.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<BaseResponseModel> GetToken([FromBody] AuthRequestModel requestModel)
        {
            return await _authService.GetTokenAsync(requestModel);
        }

        [HttpPost]
        [Route("signup")]
        public async Task<BaseResponseModel> RegisterNewUser([FromBody] AuthSignUpRequestModel requestModel)
        {
            return await _authService.RegisterNewUserAsync(requestModel);
        }
    }
}
