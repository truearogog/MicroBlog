using MicroBlog.Data.EF.Entities;
using MicroBlog.Data.EF.ValueGenerators;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Data.EF.Tests
{
    internal sealed class TestAppDb(DbContextOptions<TestAppDb> options) : AppDb<TestAppDb>(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseInMemoryDatabase("microblog.dev.test");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
