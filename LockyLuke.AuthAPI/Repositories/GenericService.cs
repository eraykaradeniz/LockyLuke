using LockyLuke.AuthAPI.DbContext;
using LockyLuke.AuthAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LockyLuke.AuthAPI.Repositories
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {

        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericService(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
           return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
           var entity = await _dbSet.FindAsync(id);

            if (entity != null)
                _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public void Remove(TEntity entity)
        {
           _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
           _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
  
}
