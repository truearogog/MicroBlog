using MicroBlog.Data.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF
{
    public interface IAppDb
    {
        DbSet<Post> Posts { get; set; }
        DbSet<Subscription> Subscriptions { get; set; }
        DbSet<Block> Blocks { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
