using System;
using Newtonsoft.Json;

namespace PushNotifications.Server.Internals
{
    internal class DurationStringToTimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        private const string SecondSuffix = "s";

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string stringValue)
            {
                var indexOfS = stringValue.IndexOf(SecondSuffix);
                if (indexOfS > 0 && int.TryParse(stringValue.Substring(0, indexOfS), out var parsedValue))
                {
                    return TimeSpan.FromSeconds(parsedValue);
                }
            }

            return TimeSpan.Zero;
        }

        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            var timeToLiveInSeconds = $"{(int)value.TotalSeconds}{SecondSuffix}";
            writer.WriteValue(timeToLiveInSeconds);
        }
    }
}
