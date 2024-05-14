using AutoMapper;
using AutoMapper.QueryableExtensions;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroBlog.Data.EF.Repositories
{
    public abstract class Repository<T, TEntity>(DbSet<TEntity> dbSet, IAppDb db, IMapper mapper) : IRepository<T> where TEntity : class
    {
        protected DbSet<TEntity> DbSet = dbSet;
        protected readonly IAppDb Db = db;
        protected IMapper Mapper = mapper;
        protected IConfigurationProvider MapperConfig => Mapper.ConfigurationProvider;

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            var query = DbSet.AsNoTracking()
                .ProjectTo<T>(MapperConfig);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).AsEnumerable();
            }
            else
            {
                return query.AsEnumerable();
            }
        }

        public async Task<T> Find(params object[] keys)
        {
            return Mapper.Map<T>(await DbSet.FindAsync(keys).ConfigureAwait(false));
        }

        public async Task Create(T model)
        {
            var entity = Mapper.Map<TEntity>(model);
            await DbSet.AddAsync(entity).ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CreateRange(IEnumerable<T> models)
        {
            var entities = Mapper.Map<IEnumerable<TEntity>>(models);
            await DbSet.AddRangeAsync(entities).ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(T model)
        {
            var entity = Mapper.Map<TEntity>(model);
            DbSet.Update(entity);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateRange(IEnumerable<T> models)
        {
            var entities = Mapper.Map<IEnumerable<TEntity>>(models);
            DbSet.UpdateRange(entities);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(T model)
        {
            var entity = Mapper.Map<TEntity>(model);
            if (DbSet.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public abstract Task DeleteRange(IEnumerable<T> models);
    }
}
