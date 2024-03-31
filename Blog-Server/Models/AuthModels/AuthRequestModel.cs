using System.ComponentModel.DataAnnotations;

namespace Blog_Server.Models.AuthModels
{
    public class AuthRequestModel
    {
        [Required]
        [MinLength(ModelRequirements.MinLoginLength, ErrorMessage = $"Login cannot be shorter than {ModelRequirements.MinLoginLengthString} characters")]
        [MaxLength(ModelRequirements.MaxLoginLength, ErrorMessage = $"Login cannot exceed {ModelRequirements.MaxLoginLengthString} characters")]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
