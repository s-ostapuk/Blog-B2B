namespace Blog_Server.Models.Database.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int PostId { get; set; }
        public BlogPost Post { get; set; } = null!;
    }
}
