using MicroBlog.Core.Services;
using MicroBlog.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBlog.Services.Extensions
{
    public static class IoCExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Register services
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IReactionService, ReactionService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddSingleton<IUserLoggingService, UserLoggingService>();

            return services;
        }
    }
}
