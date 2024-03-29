using Blog_Server.Database.Entities;

namespace Blog_Server.Interfaces.Repositories
{
    public interface ICommentsRepository : IBaseRepository<Comment>
    {
        Task<List<Comment>> GetPostCommentsByPostIdAsync(int postId);
        Task<int> RemovePostCommentByPostIdAndIdAsync(int postId, int id);
    }
}
