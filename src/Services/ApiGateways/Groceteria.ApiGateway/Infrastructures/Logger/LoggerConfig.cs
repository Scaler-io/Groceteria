using Destructurama;
using Groceteria.ApiGateway.Infrastructures.ElasticSearch;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Groceteria.ApiGateway.Infrastructures.Logger
{
    public class LoggerConfig
    {
        public static ILogger Configure(IConfiguration configuration, string logIndexPattern)
        {
            var loggerConfigOption = new LoggerConfigOptions();
            configuration.GetSection("LoggerConfigOptions").Bind(loggerConfigOption);
            var elasticUri = configuration["Elasticsearch:Uri"];

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
