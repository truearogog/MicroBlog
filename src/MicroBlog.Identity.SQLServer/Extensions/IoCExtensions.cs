using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBlog.Identity.SQLServer.Extensions
{
    public static class IoCExtensions
    {
        public static IServiceCollection AddIdentityEF(this IServiceCollection services, string connectionString, 
            Action<DbContextOptionsBuilder>? dbOptions = null, int poolSize = 128)
        {
            services.AddDbContextPool<SQLServerIdentityDb>(o =>
            {
                o.UseSqlServer(connectionString);
                dbOptions?.Invoke(o);
            }, poolSize);

            return services;
        }
    }
}
