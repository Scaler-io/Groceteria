using Serilog.Sinks.Elasticsearch;

namespace Groceteria.ApiGateway.Infrastructures.ElasticSearch
{
    public class ElasticSearchLogConfiguration
    {
        public static ElasticsearchSinkOptions ConfigureElasticSink(string elasticUri, string logIndexPattern)
        {
            return new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = logIndexPattern
            };
        }
    }
}
