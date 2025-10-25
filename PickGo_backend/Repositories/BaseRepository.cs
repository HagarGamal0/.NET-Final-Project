using Microsoft.EntityFrameworkCore;
using PickGo_backend.Context;
using PickGo_backend.Repository;
using System.Collections.Generic;

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
        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}