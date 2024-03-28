using Blog_Server.Models.ResponseModels;

namespace Blog_Server.Models.AuthModels
{
    public class AuthResponseModel : BaseResponseModel
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
