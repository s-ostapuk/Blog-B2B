using Blog_Server.Database;
using Blog_Server.Database.Entities;
using Blog_Server.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog_Server.Repositories
{
    public class CommentsRepository : BaseRepository<Comment>, ICommentsRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetPostCommentsByPostIdAsync(int postId)
        {
            return await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
        }

        public async Task<int> RemovePostCommentByPostIdAndIdAsync(int postId, int id)
        {
            return await _context.Comments.Where(c => c.Id == id && c.PostId == postId).ExecuteDeleteAsync();
        }
    }
}
