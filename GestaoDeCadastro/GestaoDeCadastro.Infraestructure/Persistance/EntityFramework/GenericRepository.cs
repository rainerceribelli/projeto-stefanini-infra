using GestaoDeCadastro.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeCadastro.Infraestructure.Persistance.EntityFramework
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbset;

        public GenericRepository(TContext context)
        {
            _dbset = context.Set<TEntity>();
            _context = context;
        }
        public IQueryable<TEntity> GetAll()
        {
            return _dbset;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public virtual void Add(TEntity entity)
        {
            _dbset.Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbset.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dbset.Update(entity);
        }

    }
}
