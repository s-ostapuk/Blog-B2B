namespace Blog_Server.Database.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
