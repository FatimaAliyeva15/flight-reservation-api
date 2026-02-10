using FlightReservation_Core.DataAccess.Abstract;
using FlightReservation_Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FlightReservation_Core.DataAccess.Concrete
{
    public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _entities;
        public BaseRepository(TContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }
        public async Task Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, params string[] incudes)
        {
            IQueryable<TEntity> query = GetQuery(incudes);
            return await query.FirstOrDefaultAsync(filter);
        }

        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, params string[] incudes)
        {
            IQueryable<TEntity> query = GetQuery(incudes);
            return filter == null ? query.ToListAsync() : query.Where(filter).ToListAsync();
        }

        public Task<List<TEntity>> GetAllPaginatedAsync(int page, int size, Expression<Func<TEntity, bool>> filter = null, params string[] incudes)
        {
            IQueryable<TEntity> query = GetQuery(incudes);
            return filter == null
                   ? query.Skip((page - 1) * size).Take(size).ToListAsync()
                   : query.Where(filter).Skip((page - 1) * size).Take(size).ToListAsync();
        }

        public async Task Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        private IQueryable<TEntity> GetQuery(params string[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
    }
}
