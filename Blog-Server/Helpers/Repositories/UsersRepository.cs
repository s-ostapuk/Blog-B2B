using Blog_Server.Interfaces.Repositories;
using Blog_Server.Models.Database;
using Blog_Server.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog_Server.Helpers.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByLoginAsync(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            return user;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
