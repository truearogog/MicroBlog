using MicroBlog.Data.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF
{
    public abstract class AppDb<TDb>(DbContextOptions<TDb> options) : DbContext(options), IAppDb where TDb : AppDb<TDb>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Block> Blocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().HasKey(x => x.Id);
            modelBuilder.Entity<Post>().HasIndex(x => x.UserId);

            modelBuilder.Entity<Subscription>().HasKey(x => new { x.FromUserId, x.ToUserId });

            modelBuilder.Entity<Block>().HasKey(x => new { x.UserId, x.BlockedUserId });
        }
    }
}
