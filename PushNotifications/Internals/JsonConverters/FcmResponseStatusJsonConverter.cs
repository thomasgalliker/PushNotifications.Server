using System;
using Newtonsoft.Json;
using PushNotifications.Google.Legacy;

namespace PushNotifications.Internals.JsonConverters
{
    internal class FcmResponseStatusJsonConverter : JsonConverter<FcmResponseStatus>
    {
        public override FcmResponseStatus ReadJson(JsonReader reader, Type objectType, FcmResponseStatus existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string stringValue)
            {
                return new FcmResponseStatus(stringValue);
            }

            return FcmResponseStatus.Unknown;
        }

        public override void WriteJson(JsonWriter writer, FcmResponseStatus value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
