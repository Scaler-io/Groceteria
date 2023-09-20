using System.Text.Json;
using System.Text.Json.Serialization;

namespace Groceteria.ApiGateway.Utils
{
    public class EnumToStringConverter<TEnum> : JsonConverter<TEnum> where TEnum: Enum
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (Enum.TryParse(typeof(TEnum), reader.GetString(), out var value))
            {
                return (TEnum)value;
            }

            return default;
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
