using Destructurama;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Groceteria.Infrastructure.Logger
{
    public class LoggerConfig
    {
        public static ILogger Configure(IConfiguration config)
        {
            var loggerConfigOption = new LoggerConfigOption();
            config.GetSection("LoggerConfigOoption").Bind(loggerConfigOption);

            return new LoggerConfiguration()
                        .Destructure
                        .UsingAttributes()
                        .Destructure
                        .With<LogDestructureModel>()
                        .MinimumLevel.ControlledBy(new LoggingLevelSwitch(LogEventLevel.Debug))
                        .MinimumLevel.Override(loggerConfigOption.OverrideSource, LogEventLevel.Warning)
                        .WriteTo.Console(outputTemplate: loggerConfigOption.OutputTemplate)
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty(nameof(Environment.MachineName), Environment.MachineName)
                        .Enrich.WithEnvironmentName()
                        .Enrich.WithMachineName()
                       .CreateLogger(); 
        }
    }
}
