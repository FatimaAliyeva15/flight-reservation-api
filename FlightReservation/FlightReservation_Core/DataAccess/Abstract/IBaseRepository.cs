using FlightReservation_Core.Entities.Abstract;


using System.Linq.Expressions;



namespace FlightReservation_Core.DataAccess.Abstract
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity, new()
    {
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, params string[] incudes);
        Task<List<TEntity>> GetAllPaginatedAsync(int page, int size, Expression<Func<TEntity, bool>> filter = null, params string[] incudes);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, bool includeDeleted = false, params string[] incudes);
        Task<List<TEntity>> GetDeletedAsync(params string[] includes);
        Task AddAsync(TEntity entity);
        Task Update(TEntity entity);
        Task SoftDeleteAsync(TEntity entity);
        Task HardDeleteAsync(TEntity entity);
        Task RecoverAsync(TEntity entity);
        Task<bool> IsExistsAsync(Guid id);
    }
}
