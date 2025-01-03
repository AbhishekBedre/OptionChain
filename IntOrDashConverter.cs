namespace OptionChain
{
    using Newtonsoft.Json;
    using System;

    using Newtonsoft.Json;
    using System;

    public class IntOrDashConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int?) || objectType == typeof(int);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String && reader.Value.ToString() == "-")
            {
                return 0; // Replace "-" with 0
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                return Convert.ToInt32(reader.Value); // Handle integer values
            }
            else if (reader.TokenType == JsonToken.String && int.TryParse(reader.Value.ToString(), out int result))
            {
                return result; // Handle numeric strings like "123"
            }

            throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing an integer value.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null || (int)value == 0)
            {
                writer.WriteValue("-"); // Serialize 0 as "-"
            }
            else
            {
                writer.WriteValue(value); // Serialize the integer value
            }
        }
    }


}
