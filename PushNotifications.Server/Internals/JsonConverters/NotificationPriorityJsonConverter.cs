using System;
using Newtonsoft.Json;
using PushNotifications.Server.Google;

namespace PushNotifications.Server.Internals.JsonConverters
{
    internal class NotificationPriorityJsonConverter : JsonConverter<NotificationPriority>
    {
        public override NotificationPriority ReadJson(JsonReader reader, Type objectType, NotificationPriority existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string stringValue)
            {
                return new NotificationPriority(stringValue);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, NotificationPriority value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
