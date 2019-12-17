using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using WebShop.Data;
using WebShop.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WebShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<WebShopContext>();
                    DataBaseInitializer.Initialize(context);
                    var userManager = services.GetRequiredService<UserManager<ShopUser>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    Task t = RoleInitializer.InitializeAsync(userManager, rolesManager, context);
                    t.Wait();
                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An erroк occured while seeding the database!");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
