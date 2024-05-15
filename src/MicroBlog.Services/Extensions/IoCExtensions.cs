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
            services.AddTransient<IPostService, PostService>();

            return services;
        }
    }
}
