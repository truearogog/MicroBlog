using MicroBlog.Data.EF.Extensions;
using MicroBlog.Data.EF.SQLServer.Extensions;
using MicroBlog.Identity.Extensions;
using MicroBlog.Identity.Models;
using MicroBlog.Identity.SQLServer;
using MicroBlog.Identity.SQLServer.Extensions;
using MicroBlog.Services.Extensions;
using MicroBlog.Web.Extensions;
using MicroBlog.Web.Middleware;
using MicroBlog.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace MicroBlog.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddWorkers();
            builder.Services.AddServices();
            builder.Services.AddDataServices();
            builder.Services.AddAppEF(builder.Configuration.GetConnectionString("AppDb") ?? throw new InvalidOperationException("Connection string 'AppDb' not found."),
                o => o.UseLazyLoadingProxies());

            builder.Services.AddIdentityServices();
            builder.Services.AddIdentityEF(builder.Configuration.GetConnectionString("IdentityDb") ?? throw new InvalidOperationException("Connection string 'IdentityDb' not found."));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedEmail = false)
                .AddEntityFrameworkStores<SQLServerIdentityDb>();

            builder.Services.AddScoped<ImageService>();
            builder.Services.AddHtmlSanitizer();

            builder.Services.AddRazorPages();
            builder.Services.AddControllers();
            builder.Services.AddMvcCore();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseMiddleware<UserLoggingMiddleware>();

            app.MapControllers();
            app.MapRazorPages();

            app.Run();
        }
    }
}
