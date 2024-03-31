using System.ComponentModel.DataAnnotations;

namespace Blog_Server.Models.RequestModels
{
    public class UpdatePostRequestModel
    {
        [Required]
        [MaxLength(ModelRequirements.MaxPostTitleLength, ErrorMessage = $"Title cannot exceed {ModelRequirements.MaxPostTitleLengthString} characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(ModelRequirements.MaxPostContentLength, ErrorMessage = $"Content cannot exceed {ModelRequirements.MaxPostContentLengthString} characters")]
        public string Content { get; set; } = string.Empty;
    }
}
