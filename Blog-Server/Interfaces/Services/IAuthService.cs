using Blog_Server.Models.AuthModels;

namespace Blog_Server.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseModel> GetTokenAsync(AuthRequestModel requestModel);
    }
}
