using Microsoft.EntityFrameworkCore;
using PickGo_backend.Context;
using PickGo_backend.Repository;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PickGo_backend.Repositries
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected DelieveryAppContext _context;
        protected DbSet<TEntity> _table;

        public BaseRepository(DelieveryAppContext context)
        {
            _context = context;
            _table = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _table.ToListAsync();
        public async Task<TEntity> GetByIdAsync(int id) => await _table.FindAsync(id);
        public async Task AddAsync(TEntity entity) => await _table.AddAsync(entity);
        public void Update(TEntity entity) => _table.Update(entity);
        public void Delete(TEntity entity) => _table.Remove(entity);

        public async Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _table.FirstOrDefaultAsync(predicate);
        }

        // ------------------------------
        // Added for controller compatibility
        // ------------------------------

        // Equivalent to GetByIdAsync
        public async Task<TEntity?> GetAsync(int id)
        {
            return await _table.FindAsync(id);
        }

        // Delete by ID (controller friendly)
        public async Task DeleteAsync(int id)
        {
            var entity = await _table.FindAsync(id);
            if (entity != null)
                _table.Remove(entity);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            throw new NotImplementedException();
        }
    }
}
