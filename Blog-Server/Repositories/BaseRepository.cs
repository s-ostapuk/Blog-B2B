using Blog_Server.Database;
using Blog_Server.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog_Server.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Collection item insertion
        /// </summary>
        /// <param name="entity">Entity item</param>
        /// <returns>Inserted item</returns>
        public virtual async Task<TEntity?> InsertAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
            }
            catch (Exception ex)
            {
                return null;
            }
            return entity;
        }
        /// <summary>
        /// Collection item update
        /// </summary>
        /// <param name="entityToUpdate">Updatable entity</param>
        /// <param name="disableChangeTracker">Change tracker value</param>
        /// <returns>Updated entity</returns>
        public virtual async Task<TEntity?> UpdateAsync(TEntity entityToUpdate, bool disableChangeTracker)
        {
            var changeTrackerStatus = _context.ChangeTracker.AutoDetectChangesEnabled;

            if (disableChangeTracker)
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
            }
            else
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = true;
            }

            if (_context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                dbSet.Attach(entityToUpdate);
            }

            _context.Entry(entityToUpdate).State = EntityState.Modified;

            _context.ChangeTracker.AutoDetectChangesEnabled = changeTrackerStatus;
            return entityToUpdate;
        }
        /// <summary>
        /// Getting all items from collection
        /// </summary>
        /// <returns>Requested entity List</returns>
        public virtual async Task<List<TEntity>?> GetAllItemsAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
    }
}
