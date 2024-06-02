using MicroBlog.Identity.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace MicroBlog.Identity.Extensions
{
    public static class IoCExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, ServiceLifetime scope = ServiceLifetime.Scoped)
        {
            // Register services
            services.Add(new ServiceDescriptor(typeof(UserManager), typeof(UserManager), scope));
            services.Add(new ServiceDescriptor(typeof(IUserManager), typeof(UserManager), scope));

            return services;
        }
    }
}
