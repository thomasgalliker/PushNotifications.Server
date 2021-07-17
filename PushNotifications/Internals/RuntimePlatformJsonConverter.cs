using System;
using Newtonsoft.Json;

namespace PushNotifications
{
    internal class RuntimePlatformJsonConverter : JsonConverter<RuntimePlatform>
    {
        public override void WriteJson(JsonWriter writer, RuntimePlatform value, JsonSerializer serializer)
        {
            if (value == null)
            {
                return;
            }

            writer.WriteValue(value.ToString());
        }

        public override RuntimePlatform ReadJson(JsonReader reader, Type objectType, RuntimePlatform existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonString = (string)reader.Value;
            if (jsonString == null)
            {
                return null;
            }

            return new RuntimePlatform(jsonString);
        }
    }
}