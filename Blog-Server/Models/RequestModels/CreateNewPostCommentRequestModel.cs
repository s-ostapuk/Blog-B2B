using System.ComponentModel.DataAnnotations;

namespace Blog_Server.Models.RequestModels
{
    public class CreateNewPostCommentRequestModel
    {
        [Required]
        [MaxLength(ModelRequirements.MaxCommentLength, ErrorMessage = $"CommentText cannot exceed {ModelRequirements.MaxCommentLengthString} characters")]
        public string CommentText { get; set; } = string.Empty;
    }
}
