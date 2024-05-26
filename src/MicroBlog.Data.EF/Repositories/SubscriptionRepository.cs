using AutoMapper;
using MicroBlog.Core.Models;
using MicroBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF.Repositories
{
    public class SubscriptionRepository(IAppDb db, IMapper mapper) : 
        Repository<Subscription, Entities.Subscription>(db.Subscriptions, db, mapper), ISubscriptionRepository
    {
        public override async Task DeleteRange(IEnumerable<Subscription> models)
        {
            await DbSet.Where(x => models.Any(y => x.ToUserId == y.ToUserId && x.FromUserId == y.FromUserId))
                .ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }


        public async Task DeleteForUser(string userId)
        {
            await DbSet.Where(x => x.FromUserId == userId || x.ToUserId == userId).ExecuteDeleteAsync().ConfigureAwait(false);
            await Db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}