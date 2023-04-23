using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
namespace CRUDConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var databaseCommand = services.GetRequiredService<DatabaseCommand>();
                    databaseCommand.InitializeDatabase();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initializing the database.");
                }
            }

            host.Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(configure =>
                    {
                        configure.AddConsole();
                    });

                    services.AddSingleton<DatabaseCommand>();

                    services.AddSingleton<IAppCommand, ListCommand>();
                    services.AddSingleton<IAppCommand, AddCommand>();
                    services.AddSingleton<IAppCommand, UpdateCommand>();
                    services.AddSingleton<IAppCommand, DeleteCommand>();

                    services.AddSingleton<AppController>();
                });
    }
}
