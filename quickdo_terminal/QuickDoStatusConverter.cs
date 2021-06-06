using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace quickdo_terminal
{
    public class QuickDoStatusConverter : JsonConverter<QuickDoStatus>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(QuickDoStatus);
        }

        public override QuickDoStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Enum.Parse<QuickDoStatus>(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, QuickDoStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Enum.GetName(value));
        }
    }
}