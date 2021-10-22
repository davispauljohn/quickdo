using quickdo_terminal.Types;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace quickdo_terminal
{
    public class RankedCollectionConverter : JsonConverter<RankedCollection<Task>>
    {
        public override RankedCollection<Task> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, RankedCollection<Task> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}