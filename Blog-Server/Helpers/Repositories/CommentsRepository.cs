using Blog_Server.Interfaces.Repositories;
using Blog_Server.Models.Database;
using Blog_Server.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog_Server.Helpers.Repositories
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
            var comments = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
            return comments;
        }

        public async Task<int> RemovePostCommentByPostIdAndIdAsync(int postId, int id)
        {
            return await _context.Comments.Where(c => c.Id == id && c.PostId == postId).ExecuteDeleteAsync();
        }
    }
}
