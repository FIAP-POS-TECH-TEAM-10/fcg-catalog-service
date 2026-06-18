using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories.Shared
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private DbContext _context;
        protected DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }     
        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }
        public TEntity? Get(params object[] key)
        {
            return _context.Set<TEntity>().Find(key);
        }
        public TEntity? Primeiro(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).FirstOrDefault();
        }
        public void Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(Func<TEntity, bool> predicate)
        {
            _context.Set<TEntity>()
           .Where(predicate).ToList()
           .ForEach(del => _context.Set<TEntity>().Remove(del));
        }
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
            GC.SuppressFinalize(this);
        }       
    }
}