using System.Linq.Expressions;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        TEntity? Get(params object[] key);       
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(Func<TEntity, bool> predicate);
        void Dispose();
    }
}
