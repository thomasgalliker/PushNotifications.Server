
using System;
using Newtonsoft.Json;
using PushNotifications.Server.Apple;

namespace PushNotifications.Server.Internals
{
    internal class ApnsResponseReasonJsonConverter : JsonConverter<ApnsResponseReason>
    {
        public override ApnsResponseReason ReadJson(JsonReader reader, Type objectType, ApnsResponseReason existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string stringValue)
            {
                return new ApnsResponseReason(stringValue);
            }

            return ApnsResponseReason.Unknown;
        }

        public override void WriteJson(JsonWriter writer, ApnsResponseReason value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}