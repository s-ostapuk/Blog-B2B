using Blog_Server.Interfaces.Repositories;
using Blog_Server.Models.AuthModels;
using Blog_Server.Models.Database;
using Blog_Server.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog_Server.Helpers.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        #region Properties
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructors
        public UsersRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }
        #endregion
        
        #region Public Methods
        public async Task<User?> GetUserByLoginAsync(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            return user;
        }
        #endregion
    }
}
