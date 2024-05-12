using AutoMapper;
using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF.Repositories
{
    public class PostRepository(IAppDb db, IMapper mapper) : Repository<Post, Entities.Post>(db.Posts, db, mapper), IRepository<Post>
    {
        public override async Task DeleteRange(IEnumerable<Post> models)
        {
            var ids = models.Select(x => x.Id).ToHashSet();
            await DbSet.Where(x => ids.Contains(x.Id))
                .ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
