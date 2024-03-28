using Blog_Server.Models.Database.Entities;

namespace Blog_Server.Interfaces.Repositories
{
    public interface IBlogPostsRepository : IBaseRepository<BlogPost>
    {
        Task<BlogPost?> GetPostByIdAsync(int id);
        Task<int> RemovePostByIdAsync(int id);
    }
}
