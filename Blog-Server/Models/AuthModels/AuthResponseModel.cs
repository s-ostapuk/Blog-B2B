using Blog_Server.Models.DtoModels;
using Blog_Server.Models.ResponseModels;

namespace Blog_Server.Models.AuthModels
{
    public class AuthResponseDataModel 
    {
        public string AccessToken { get; set; } = string.Empty;
        public UserDto UserInfo { get; set; } = null;
    }
}
