using System.ComponentModel.DataAnnotations;

namespace Blog_Server.Models.AuthModels
{
    public class AuthSignUpRequestModel
    {
        [Required]
        [MinLength(ModelRequirements.MinLoginLength, ErrorMessage = $"Login cannot be shorter than {ModelRequirements.MinLoginLengthString} characters")]
        [MaxLength(ModelRequirements.MaxLoginLength, ErrorMessage = $"Login cannot exceed {ModelRequirements.MaxLoginLengthString} characters")]
        public string Login { get; set; } = string.Empty;
        [Required]
        [MinLength(ModelRequirements.MinPasswordLength, ErrorMessage = $"Password cannot be shorter than {ModelRequirements.MinPasswordLengthString} characters")]
        public string Password { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}
