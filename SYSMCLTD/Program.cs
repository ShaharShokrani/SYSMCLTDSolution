using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SYSMCLTD
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
                    var context = services.GetRequiredService<SYSMCLTDDbContext>();
                    if (context.Database.GetPendingMigrations().Any())                    
                        context.Database.Migrate();
                    DoSeed(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured during migration");
                }
            }
                
             host.Run();
        }

        private static void DoSeed(SYSMCLTDDbContext context)
        {
            if (!context.Customers.Any(x => !x.IsDeleted))
            {
                var userData = System.IO.File.ReadAllText("SeedingData/Customers.json");
                var users = JsonConvert.DeserializeObject<List<Customer>>(userData);
                foreach (var user in users)
                {
                    context.Customers.Add(user);
                }

                context.SaveChanges();
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
