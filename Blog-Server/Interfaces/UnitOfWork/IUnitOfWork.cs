using Blog_Server.Interfaces.Repositories;
using Blog_Server.Models.Database;

namespace Blog_Server.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository UsersRepository { get; }
        IBlogPostsRepository BlogPostsRepository { get; }
        ICommentsRepository CommentsRepository { get; }
        Task CompleteAsync();
    }
}
