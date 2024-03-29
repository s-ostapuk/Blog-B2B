using Blog_Server.Interfaces.Repositories;
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
