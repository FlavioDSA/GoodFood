using System;
using GoodFood.Recipes.Application.Users.Model;
using GoodFood.Recipes.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GoodFood.Receipts.Api
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var recipesDbContext = scope.ServiceProvider.GetRequiredService<RecipesDbContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    recipesDbContext.Database.Migrate();
                    SeedDb.SeedAsync(recipesDbContext, userManager).Wait();
                }
                catch (Exception ex)
                {
                    scope.ServiceProvider.GetRequiredService<ILogger>().LogError(ex, "Error running the app");
                }
            }

            host.Run();
        }
    }
}