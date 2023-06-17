using Groceteria.NotificationMessgae.Processor.DependencyInjections;
using Groceteria.NotificationMessgae.Processor.Extensions;
using Groceteria.NotificationMessgae.Processor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Groceteria.NotificationMessgae.Processor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.MigrateDbAsync();
            await host.RunAsync();
        }
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                    services.ConfigurationSetting();
                    services.AddHostedService<BackgroundNotificationService>();
                    services.AddApplicationServices(configuration)
                    .AddDataServices(configuration);

                }).UseSerilog();
        }
    }
}
