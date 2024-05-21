using MicroBlog.Core.Services;
using MicroBlog.Identity.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MicroBlog.Workers.Workers
{
    internal class UserLoggingWorker(IServiceProvider serviceProvider, IUserLoggingService userLoggingService) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly IUserLoggingService _userLoggingService = userLoggingService;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager>();

                var userLogins = _userLoggingService.GetLogins();
                foreach (var (userId, lastSeen) in userLogins)
                {
                    var user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        await userManager.SetLastSeenAsync(user, lastSeen);
                    }
                }
                _userLoggingService.ResetLogins();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
