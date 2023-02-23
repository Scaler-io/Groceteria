using Destructurama;
using Groceteria.Infrastructure.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Groceteria.Infrastructure.Logger
{
    public class LoggerConfig
    {
        public static ILogger Configure(IConfiguration config, 
            string logIndexPattern
        )
        {
            var loggerConfigOption = new LoggerConfigOption();
            config.GetSection("LoggerConfigOption").Bind(loggerConfigOption);
            var elasticUri = config["Elasticsearch:Uri"];

            return new LoggerConfiguration()
                        .Destructure
                        .UsingAttributes()
                        .Destructure
                        .With<LogDestructureModel>()
                        .MinimumLevel.ControlledBy(new LoggingLevelSwitch(LogEventLevel.Debug))
                        .MinimumLevel.Override(loggerConfigOption.OverrideSource, LogEventLevel.Warning)
                        .WriteTo.Console(outputTemplate: loggerConfigOption.OutputTemplate)
                        .WriteTo.Elasticsearch(ElasticSearchLogConfiguration.ConfigureElasticSink(elasticUri, logIndexPattern))
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty(nameof(Environment.MachineName), Environment.MachineName)
                        .Enrich.WithEnvironmentName()
                        .Enrich.WithMachineName()
                       .CreateLogger(); 
        }
    }
}
