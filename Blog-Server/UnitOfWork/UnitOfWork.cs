using Blog_Server.Repositories;
using Blog_Server.Interfaces.Repositories;
using Blog_Server.Interfaces.UnitOfWork;
using Blog_Server.Database;

namespace Blog_Server.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IBlogPostsRepository blogPostsRepository;
        private ICommentsRepository commentsRepository;
        private IUsersRepository usersRepository;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IBlogPostsRepository BlogPostsRepository => blogPostsRepository = blogPostsRepository ?? new BlogPostsRepository(_context);
        public IUsersRepository UsersRepository => usersRepository = usersRepository ?? new UsersRepository(_context);
        public ICommentsRepository CommentsRepository => commentsRepository = commentsRepository ?? new CommentsRepository(_context);

        /// <summary>
        /// Apply repository changes
        /// </summary>
        /// <returns></returns>
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
