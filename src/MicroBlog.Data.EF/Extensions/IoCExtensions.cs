using AutoMapper;
using MicroBlog.Core.Repositories;
using MicroBlog.Data.EF.Profiles;
using MicroBlog.Data.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBlog.Data.EF.Extensions
{
    public static class IoCExtensions
    {
        /// <summary>
        /// Adds the DbContext and the default services needed to run
        /// an application over Entity Framework Core.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <param name="dboptions">The DbContext options builder</param>
        /// <param name="poolSize">The optional connection pool size. Default value is 128</param>
        /// <param name="scope">The optional lifetime</param>
        /// <typeparam name="T">The DbContext type</typeparam>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddAppEF<T>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> dboptions, int poolSize = 128, ServiceLifetime scope = ServiceLifetime.Scoped) where T : DbContext, IAppDb
        {
            services.AddDbContextPool<T>(dboptions, poolSize);

            return RegisterServices<T>(services, scope);
        }

        /// <summary>
        /// Adds the default services needed to run an application over Entity Framework Core.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <param name="scope">The optional lifetime</param>
        /// <typeparam name="T">The DbContext type</typeparam>
        /// <returns>The updated service collection</returns>
        private static IServiceCollection RegisterServices<T>(this IServiceCollection services,
            ServiceLifetime scope = ServiceLifetime.Scoped) where T : DbContext, IAppDb
        {
            // Register repositories
            services.Add(new ServiceDescriptor(typeof(IPostRepository), typeof(PostRepository), scope));
            services.Add(new ServiceDescriptor(typeof(ISubscriptionRepository), typeof(SubscriptionRepository), scope));
            services.Add(new ServiceDescriptor(typeof(IBlockRepository), typeof(BlockRepository), scope));

            // Register services
            services.Add(new ServiceDescriptor(typeof(IAppDb), typeof(T), scope));

            // Register AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(PostProfile));
                cfg.AddProfile(typeof(SubscriptionProfile));
                cfg.AddProfile(typeof(BlockProfile));
            });
            services.Add(new ServiceDescriptor(typeof(IMapper), new Mapper(config)));

            return services;
        }
    }
}
