using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.CodeAnalysis;

namespace Groceteria.ApiGateway.Infrastructures.Logger
{
    public class LogDestructureModel : IDestructuringPolicy
    {
        public bool TryDestructure(object value, 
            ILogEventPropertyValueFactory propertyValueFactory, 
            [NotNullWhen(true)] out LogEventPropertyValue? result)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            result = new ScalarValue(JsonConvert.SerializeObject(value, jsonSettings));
            return true;
        }
    }
}
