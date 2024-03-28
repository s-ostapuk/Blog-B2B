using Blog_Server.Models.AuthModels;
using Blog_Server.Models.Database.Entities;

namespace Blog_Server.Interfaces.Repositories
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<User?> GetUserByLoginAsync(string login);
    }
}
