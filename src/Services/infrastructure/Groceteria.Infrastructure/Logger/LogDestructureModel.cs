using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog.Core;
using Serilog.Events;

namespace Groceteria.Infrastructure.Logger
{
    public class LogDestructureModel : IDestructuringPolicy
    {
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
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
