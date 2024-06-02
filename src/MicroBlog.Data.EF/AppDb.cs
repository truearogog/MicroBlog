using MicroBlog.Data.EF.Entities;
using MicroBlog.Data.EF.ValueGenerators;
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
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().HasKey(x => x.Id);
            modelBuilder.Entity<Post>().HasIndex(x => x.UserId);
            modelBuilder.Entity<Post>().Property(x => x.Created)
                .HasValueGenerator<CurrentTimeValueGenerator>().ValueGeneratedOnAdd();
            modelBuilder.Entity<Post>().Property(x => x.Updated)
                .HasValueGenerator<CurrentTimeValueGenerator>().ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Subscription>().HasKey(x => new { x.FromUserId, x.ToUserId });

            modelBuilder.Entity<Block>().HasKey(x => new { x.UserId, x.BlockedUserId });

            modelBuilder.Entity<Reaction>().HasKey(x => new { x.PostId, x.UserId, x.Type });
            modelBuilder.Entity<Reaction>().HasOne(x => x.Post).WithMany(x => x.Reactions).HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>().HasKey(x => x.Id);
            modelBuilder.Entity<Comment>().HasIndex(x => x.PostId);
            modelBuilder.Entity<Comment>().HasOne(x => x.Post).WithMany(x => x.Comments).HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comment>().Property(x => x.Created)
                .HasValueGenerator<CurrentTimeValueGenerator>().ValueGeneratedOnAdd();
            modelBuilder.Entity<Comment>().Property(x => x.Updated)
                .HasValueGenerator<CurrentTimeValueGenerator>().ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Image>().HasKey(x => x.Path);
            modelBuilder.Entity<Image>().HasIndex(x => x.UserId);
        }
    }
}
