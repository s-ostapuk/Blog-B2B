using Blog_Server.Interfaces;
using Blog_Server.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog_Server.Database.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength( ModelRequirements.MaxLoginLength)]
        public string Login { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
