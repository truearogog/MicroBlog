using AutoMapper;
using MicroBlog.Core.Repositories;
using MicroBlog.Data.EF.Profiles;
using MicroBlog.Data.EF.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBlog.Data.EF.Extensions
{
    public static class IoCExtensions
    {
        /// <summary>
        /// Adds the default services needed to run an application over Entity Framework Core.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <param name="scope">The optional lifetime</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddDataServices(this IServiceCollection services,
            ServiceLifetime scope = ServiceLifetime.Scoped)
        {
            // Register repositories
            services.Add(new ServiceDescriptor(typeof(IPostRepository), typeof(PostRepository), scope));
            services.Add(new ServiceDescriptor(typeof(ISubscriptionRepository), typeof(SubscriptionRepository), scope));
            services.Add(new ServiceDescriptor(typeof(IBlockRepository), typeof(BlockRepository), scope));
            services.Add(new ServiceDescriptor(typeof(IReactionRepository), typeof(ReactionRepository), scope));
            services.Add(new ServiceDescriptor(typeof(ICommentRepository), typeof(CommentRepository), scope));
            services.Add(new ServiceDescriptor(typeof(IImageRepository), typeof(ImageRepository), scope));

            // Register AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(PostProfile));
                cfg.AddProfile(typeof(SubscriptionProfile));
                cfg.AddProfile(typeof(BlockProfile));
                cfg.AddProfile(typeof(ReactionProfile));
                cfg.AddProfile(typeof(CommentProfile));
                cfg.AddProfile(typeof(ImageProfile));
            });
            services.AddSingleton<IMapper>(new Mapper(config));

            return services;
        }
    }
}
