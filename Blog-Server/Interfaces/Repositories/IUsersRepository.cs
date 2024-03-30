using Blog_Server.Database.Entities;

namespace Blog_Server.Interfaces.Repositories
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByLoginAsync(string login);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
