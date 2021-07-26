using System;
using Newtonsoft.Json;

namespace PushNotifications.Internals
{
    internal class UnixTimestampMillisecondsJsonConverter : JsonConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? ReadJson(JsonReader reader, Type objectType, DateTimeOffset? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is long longValue)
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(longValue);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, DateTimeOffset? value, JsonSerializer serializer)
        {
            if (value is DateTimeOffset dateTimeOffsetValue)
            {
                writer.WriteValue(dateTimeOffsetValue.ToUnixTimeMilliseconds());
            }
        }
    }
}