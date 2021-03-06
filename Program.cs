using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using udemyApp.API.Data;
using udemyApp.API.Helpers;

namespace udemyApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    context.Database.Migrate();
                    Seed.SeedUsers(context);
                }
                catch (Exception error)
                {
                    var context = services.GetRequiredService<DataContext>();
                    var function = "Main";
                    var page = "Program";
                    var user = ".NetCore";
                    Extensions.AddToApplicationLog(error.Message, error.Source, error.StackTrace, function, page, user, context);
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(error, "An error occured during migration");
                }

                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}