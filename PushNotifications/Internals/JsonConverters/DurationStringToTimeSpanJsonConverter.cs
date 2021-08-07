
using System;
using Newtonsoft.Json;

namespace PushNotifications.Internals
{
    internal class DurationStringToTimeSpanJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is TimeSpan timeSpan))
            {
                return;
            }

            var timeToLiveInSeconds = string.Format("{0}s", (int)timeSpan.TotalSeconds);
            writer.WriteValue(timeToLiveInSeconds);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TimeSpan?) == objectType;
        }
    }
}
