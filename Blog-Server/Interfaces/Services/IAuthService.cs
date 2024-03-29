using Blog_Server.Models.AuthModels;
using Blog_Server.Models.ResponseModels;

namespace Blog_Server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseModel> GetTokenAsync(AuthRequestModel requestModel);
        Task<BaseResponseModel> RegisterNewUserAsync(AuthSignUpRequestModel requestModel);
    }
}
