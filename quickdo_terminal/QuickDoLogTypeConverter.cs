using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace quickdo_terminal
{
    public class QuickDoLogTypeConverter : JsonConverter<QuickDoLogType>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(QuickDoStatus);
        }

        public override QuickDoLogType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Enum.Parse<QuickDoLogType>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, QuickDoLogType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Enum.GetName(value));
        }
    }
}