using AutoMapper;
using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MicroBlog.Data.EF.Repositories
{
    public class PostRepository(IAppDb db, IMapper mapper) : 
        Repository<Post, Entities.Post>(db.Posts, db, mapper), IPostRepository
    {
        public override IEnumerable<Post> GetAll(Expression<Func<Post, bool>>? filter = null, Func<IQueryable<Post>, IOrderedQueryable<Post>>? orderBy = null)
        {
            orderBy ??= x => x.OrderByDescending(y => y.Created);
            return base.GetAll(filter, orderBy);
        }

        public override async Task DeleteRange(IEnumerable<Post> models)
        {
            var ids = models.Select(x => x.Id).ToHashSet();
            await DbSet.Where(x => ids.Contains(x.Id))
                .ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
