using System;
using Newtonsoft.Json;
using PushNotifications.Server.Google;

namespace PushNotifications.Server.Internals.JsonConverters
{
    internal class AndroidMessagePriorityJsonConverter : JsonConverter<AndroidMessagePriority>
    {
        public override AndroidMessagePriority ReadJson(JsonReader reader, Type objectType, AndroidMessagePriority existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string stringValue)
            {
                return new AndroidMessagePriority(stringValue);
            }

            return AndroidMessagePriority.Unknown;
        }

        public override void WriteJson(JsonWriter writer, AndroidMessagePriority value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
