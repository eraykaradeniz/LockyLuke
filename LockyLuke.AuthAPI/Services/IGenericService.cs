using System.Linq.Expressions;

namespace LockyLuke.AuthAPI.Services
{
    public interface IGenericService<TEntity> where TEntity : class
    {

        Task<TEntity> GetByIdAsync(string id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity);
        TEntity Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
