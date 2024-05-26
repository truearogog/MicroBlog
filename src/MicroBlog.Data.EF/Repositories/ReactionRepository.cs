using AutoMapper;
using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF.Repositories
{
    public class ReactionRepository(IAppDb db, IMapper mapper) :
        Repository<Reaction, Entities.Reaction>(db.Reactions, db, mapper), IReactionRepository
    {
        public override async Task DeleteRange(IEnumerable<Reaction> models)
        {
            await DbSet.Where(x => models.Any(y => x.PostId == y.PostId && x.UserId == y.UserId && x.Type == y.Type))
                .ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }


        public async Task DeleteForUser(string userId)
        {
            await DbSet.Where(x => x.UserId == userId).ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
