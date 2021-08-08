using System;
using Newtonsoft.Json;
using PushNotifications.Server.Google;

namespace PushNotifications.Server.Internals.JsonConverters
{
    internal class VisibilityJsonConverter : JsonConverter<Visibility>
    {
        public override Visibility ReadJson(JsonReader reader, Type objectType, Visibility existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string stringValue)
            {
                return new Visibility(stringValue);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, Visibility value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
