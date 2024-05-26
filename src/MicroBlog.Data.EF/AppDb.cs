using MicroBlog.Data.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF
{
    public abstract class AppDb<TDb>(DbContextOptions<TDb> options) : DbContext(options), IAppDb where TDb : AppDb<TDb>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().HasKey(x => x.Id);
            modelBuilder.Entity<Post>().HasIndex(x => x.UserId);

            modelBuilder.Entity<Subscription>().HasKey(x => new { x.FromUserId, x.ToUserId });

            modelBuilder.Entity<Block>().HasKey(x => new { x.UserId, x.BlockedUserId });

            modelBuilder.Entity<Reaction>().HasKey(x => new { x.PostId, x.UserId, x.Type });
            modelBuilder.Entity<Reaction>().HasOne(x => x.Post).WithMany(x => x.Reactions).HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Comment>().HasKey(x => x.Id);
            modelBuilder.Entity<Comment>().HasIndex(x => x.PostId);
            modelBuilder.Entity<Comment>().HasOne(x => x.Post).WithMany(x => x.Comments).HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
