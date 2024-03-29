namespace Blog_Server.Models.AuthModels
{
    public class AuthSignUpRequestModel
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set;} = string.Empty;
    }
}
