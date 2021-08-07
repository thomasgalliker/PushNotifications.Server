using System;
using Newtonsoft.Json;

namespace PushNotifications.Internals
{
    internal class BoolToIntJsonConverter : JsonConverter<bool>
    {
        public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Convert.ToBoolean(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
        {
            writer.WriteValue(Convert.ToInt32(value));
        }
    }
}
