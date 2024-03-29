using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace Blog_Server.Database.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
