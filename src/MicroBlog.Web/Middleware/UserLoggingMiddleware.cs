using MicroBlog.Core.Services;
using MicroBlog.Identity.Extensions;

namespace MicroBlog.Web.Middleware
{
    public class UserLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        private readonly RequestDelegate _next = next;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                using var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var userLoggingService = serviceScope.ServiceProvider.GetRequiredService<IUserLoggingService>();

                var userId = context.User.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    userLoggingService.AddOrUpdateLogin(userId, DateTime.UtcNow);
                }
            }

            await _next(context);
        }
    }
}
