using Blog_Server.Interfaces;
using Blog_Server.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog_Server.Database.Entities
{
    public class BlogPost : IEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(ModelRequirements.MaxPostTitleLength)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(ModelRequirements.MaxPostContentLength)]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
