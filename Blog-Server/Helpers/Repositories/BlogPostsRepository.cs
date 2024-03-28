using Blog_Server.Interfaces.Repositories;
using Blog_Server.Models.Database;
using Blog_Server.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog_Server.Helpers.Repositories
{
    public class BlogPostsRepository : BaseRepository<BlogPost>, IBlogPostsRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogPostsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BlogPost?> GetPostByIdAsync(int id)
        {
            var post = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id);
            return post;
        }

        public async Task<int> RemovePostByIdAsync(int id)
        {
            return await _context.BlogPosts.Where(p => p.Id == id).ExecuteDeleteAsync();
        }
    }
}
