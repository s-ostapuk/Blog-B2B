using Blog_Server.Database;
using Blog_Server.Database.Entities;
using Blog_Server.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog_Server.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByLoginAsync(string login)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Login == login);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
