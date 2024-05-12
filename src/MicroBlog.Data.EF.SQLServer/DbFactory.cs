using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace MicroBlog.Data.EF.SQLServer
{
    [ExcludeFromCodeCoverage]
    public class DbFactory : IDesignTimeDbContextFactory<SQLServerAppDb>
    {
        public SQLServerAppDb CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SQLServerAppDb>();
            builder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=microblog.dev;integrated security=true;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new SQLServerAppDb(builder.Options);
        }
    }
}
