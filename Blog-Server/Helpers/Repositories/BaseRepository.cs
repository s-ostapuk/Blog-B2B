using Blog_Server.Interfaces.Repositories;
using Blog_Server.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Blog_Server.Helpers.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        #region Properties
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> dbSet;
        #endregion

        #region Constructors
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }
        #endregion

        #region Virtual Methods
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

        public virtual async Task<TEntity?> UpdateAsync(TEntity entityToUpdate, bool disableChangeTracker, bool saveChanges = true)
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

        public virtual async Task<List<TEntity>?> GetAllItemsAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        #endregion
    }
}
