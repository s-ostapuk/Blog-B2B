namespace Blog_Server.Models.DtoModels
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Author { get; set; } = string.Empty;

        public int PostId { get; set; }
    }
}
