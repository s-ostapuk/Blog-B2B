using System.Text.Json.Serialization;

namespace Blog_Server.Database.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Author { get; set; } = string.Empty;

        public int PostId { get; set; }
        [JsonIgnore]
        public BlogPost Post { get; set; } = null!;
    }
}
