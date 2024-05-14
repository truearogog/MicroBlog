using MicroBlog.Data.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF
{
    public abstract class AppDb<TDb>(DbContextOptions<TDb> options) : DbContext(options), IAppDb where TDb : AppDb<TDb>
    {
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().HasKey(x => x.Id);
            modelBuilder.Entity<Post>().HasIndex(x => x.UserId);
        }
    }
}
