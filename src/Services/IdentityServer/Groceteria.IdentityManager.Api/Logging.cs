using Destructurama;
using Groceteria.IdentityManager.Api.Configurations.App;
using Groceteria.IdentityManager.Api.Configurations.Logger;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Groceteria.IdentityManager.Api
{
    public class Logging
    {
        public static ILogger GetLogger(IConfiguration configuration, IWebHostEnvironment environment)
        {
            var loggingOptions = configuration.GetSection("Logging").Get<LoggingOptions>();
            var appConfigurations = configuration.GetSection("AppConfigurations").Get<AppConfigurations>();
            var elasticUri = configuration["Elasticsearch:Uri"];
            var logIndexPattern = $"Groceteria.IdentityManager.Api-{environment.EnvironmentName}";

            Enum.TryParse(loggingOptions.Console.LogLevel, false, out LogEventLevel minimumEventLevel);

            var loggerConfigurations = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(new LoggingLevelSwitch(minimumEventLevel))
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty(nameof(Environment.MachineName), Environment.MachineName)
                .Enrich.WithProperty(nameof(appConfigurations.ApplicationIdentifier), appConfigurations.ApplicationIdentifier)
                .Enrich.WithProperty(nameof(appConfigurations.ApplicationEnvironment), appConfigurations.ApplicationEnvironment);

            if (loggingOptions.Console.Enabled)
            {
                loggerConfigurations.WriteTo.Console(minimumEventLevel, loggingOptions.LogOutputTemplate);
            }
            if (loggingOptions.Elastic.Enabled)
            {
                loggerConfigurations.WriteTo.Elasticsearch(elasticUri, logIndexPattern);
            }

            return loggerConfigurations
                   .Destructure
                   .UsingAttributes()
                   .CreateLogger();
        }
    }
}
