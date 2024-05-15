using MicroBlog.Identity.Managers;

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
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager>();
                var user = await userManager.GetUserAsync(context.User);

                if (user != null)
                {
                    await userManager.SetLastSeenAsync(user, DateTime.UtcNow);
                }
            }

            await _next(context);
        }
    }
}
