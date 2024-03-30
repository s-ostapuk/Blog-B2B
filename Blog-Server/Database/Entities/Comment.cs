using Blog_Server.Interfaces;
using Blog_Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blog_Server.Database.Entities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(ModelRequirements.MaxCommentLength)]
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [MaxLength(ModelRequirements.MaxLoginLength)]
        public string AuthorLogin { get; set; } = string.Empty;

        public int PostId { get; set; }
        [JsonIgnore]
        public BlogPost Post { get; set; } = null!;
    }
}
