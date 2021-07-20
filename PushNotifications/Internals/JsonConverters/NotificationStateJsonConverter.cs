using System;
using PushNotifications.Messages;
using Newtonsoft.Json;

namespace PushNotifications
{
    public class NotificationStateJsonConverter : JsonConverter<NotificationState>
    {
        public override void WriteJson(JsonWriter writer, NotificationState value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override NotificationState ReadJson(JsonReader reader, Type objectType, NotificationState existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}