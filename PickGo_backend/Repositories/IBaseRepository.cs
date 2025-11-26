using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PickGo_backend.Repository { 
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate);

    }
}