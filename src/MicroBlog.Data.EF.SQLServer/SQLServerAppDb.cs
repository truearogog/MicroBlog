using MicroBlog.Data.EF.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace MicroBlog.Data.EF.SQLServer
{
    [ExcludeFromCodeCoverage]
    public class SQLServerAppDb(DbContextOptions<SQLServerAppDb> options) : AppDb<SQLServerAppDb>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().Property(x => x.Created).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Post>().Property(x => x.Updated).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
