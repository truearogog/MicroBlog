using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace MicroBlog.Identity.SQLServer
{
    [ExcludeFromCodeCoverage]
    public class DbFactory : IDesignTimeDbContextFactory<SQLServerIdentityDb>
    {
        public SQLServerIdentityDb CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SQLServerIdentityDb>();
            builder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=microblog.identity.dev;integrated security=true;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new SQLServerIdentityDb(builder.Options);
        }
    }
}
