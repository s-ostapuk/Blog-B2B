namespace Blog_Server.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> InsertAsync(TEntity entity);
        Task<TEntity?> UpdateAsync(TEntity entityToUpdate, bool disableChangeTracker);
        Task<List<TEntity>?> GetAllItemsAsync();
    }
}
