using MicroBlog.Workers.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBlog.Services.Extensions
{
    public static class IoCExtensions
    {
        public static IServiceCollection AddWorkers(this IServiceCollection services)
        {
            // Register services
            services.AddHostedService<UserLoggingWorker>();

            return services;
        }
    }
}
