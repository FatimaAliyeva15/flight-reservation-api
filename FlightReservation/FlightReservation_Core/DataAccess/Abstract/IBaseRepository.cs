using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FlightReservation_Core.DataAccess.Abstract
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity, new()
    {
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, params string[] incudes);
        Task<List<TEntity>> GetAllPaginatedAsync(int page, int size, Expression<Func<TEntity, bool>> filter = null, params string[] incudes);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, params string[] incudes);
        Task AddAsync(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
    }
}
