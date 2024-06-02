using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBlog.Data.EF.SQLServer.Extensions
{
    public static class IoCExtensions
    {
        /// <summary>
        /// Adds the DbContext and the default services needed to run
        /// an application over Entity Framework Core.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <param name="dbOptions">The DbContext options builder</param>
        /// <param name="sqlServerDbOptions">The SQLServerAppDb options builder</param>
        /// <param name="poolSize">The optional connection pool size. Default value is 128</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddAppEF(this IServiceCollection services, string connectionString,
            Action<DbContextOptionsBuilder> dbOptions, Action<SqlServerDbContextOptionsBuilder>? sqlServerDbOptions = null, int poolSize = 128, ServiceLifetime scope = ServiceLifetime.Scoped)
        {
            services.AddDbContextPool<SQLServerAppDb>(o =>
            {
                o.UseSqlServer(connectionString, sqlServerDbOptions);
                dbOptions(o);
            }, poolSize);
            services.Add(new ServiceDescriptor(typeof(IAppDb), typeof(SQLServerAppDb), scope));

            return services;
        }
    }
}
