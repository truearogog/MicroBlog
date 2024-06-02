using AutoMapper;
using AutoMapper.QueryableExtensions;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroBlog.Data.EF.Repositories
{
    public abstract class Repository<T, TEntity>(DbSet<TEntity> dbSet, IAppDb db, IMapper mapper) : IRepository<T> where TEntity : class, IEquatable<TEntity>
    {
        protected DbSet<TEntity> DbSet = dbSet;
        protected readonly IAppDb Db = db;
        protected IMapper Mapper = mapper;
        protected IConfigurationProvider MapperConfig => Mapper.ConfigurationProvider;

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            var query = DbSet.AsNoTracking()
                .ProjectTo<T>(MapperConfig);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public async Task<T> Find(params object[] keys)
        {
            var entity = await DbSet.FindAsync(keys).ConfigureAwait(false);
            return Mapper.Map<T>(entity);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.ProjectTo<T>(MapperConfig)
                .AnyAsync(predicate).ConfigureAwait(false);
        }

        public async Task<bool> All(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.ProjectTo<T>(MapperConfig)
                .AllAsync(predicate).ConfigureAwait(false);
        }

        public async Task<T> Create(T model)
        {
            var entity = Mapper.Map<TEntity>(model);
            await DbSet.AddAsync(entity).ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
            return Mapper.Map<T>(entity);
        }

        public async Task<IEnumerable<T>> CreateRange(IEnumerable<T> models)
        {
            var entities = Mapper.Map<IEnumerable<TEntity>>(models);
            await DbSet.AddRangeAsync(entities).ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
            return Mapper.Map<IEnumerable<T>>(entities);
        }

        public async Task Update(T model)
        {
            var entity = Mapper.Map<TEntity>(model);
            var existingEntity = Db.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.Equals(entity));
            if (existingEntity != null)
            {
                DbSet.Entry(existingEntity.Entity).CurrentValues.SetValues(entity);
            }
            else
            {
                DbSet.Attach(entity);
            }
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateRange(IEnumerable<T> models)
        {
            var entities = Mapper.Map<IEnumerable<TEntity>>(models);
            foreach (var entity in entities)
            {
                var existingEntity = Db.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.Equals(entity));
                if (existingEntity != null)
                {
                    DbSet.Entry(existingEntity.Entity).CurrentValues.SetValues(entity);
                }
                else
                {
                    DbSet.Attach(entity);
                }
            }
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(T model)
        {
            var entity = Mapper.Map<TEntity>(model);
            var existingEntity = Db.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.Equals(entity));
            if (existingEntity != null)
            {
                DbSet.Remove(existingEntity.Entity);
            }
            else
            {
                DbSet.Remove(entity);
            }
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(params object[] keys)
        {
            var entity = await DbSet.FindAsync(keys).ConfigureAwait(false);
            if (entity != null)
            {
                DbSet.Remove(entity);
                await Db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteRange(IEnumerable<T> models)
        {
            var entities = Mapper.Map<IEnumerable<TEntity>>(models);
            foreach (var entity in entities)
            {
                var existingEntity = Db.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.Equals(entity));
                if (existingEntity != null)
                {
                    DbSet.Remove(existingEntity.Entity);
                }
                else
                {
                    DbSet.Remove(entity);
                }
            }
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
