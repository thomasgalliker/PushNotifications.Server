using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PushNotifications.Server.Internals
{
    internal class DurationStringToTimeSpanJsonConverter : JsonConverter<TimeSpan?>
    {
        private const string SecondSuffix = "s";

        public override TimeSpan? ReadJson(JsonReader reader, Type objectType, TimeSpan? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string stringValue)
            {
                return StringToTimeSpan(stringValue);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, TimeSpan? value, JsonSerializer serializer)
        {
            string stringValue = TimeSpanToString(value);
            writer.WriteValue(stringValue);
        }

        internal static string TimeSpanToString(TimeSpan? value)
        {
            if (value is TimeSpan timeSpan)
            {
                return $"{timeSpan.TotalSeconds:R9}{SecondSuffix}";
            }

            return null;
        }

        internal static TimeSpan? StringToTimeSpan(string value)
        {
            var indexOfS = value.IndexOf(SecondSuffix);
            if (indexOfS > 0)
            {
                if (double.TryParse(value.Substring(0, indexOfS), out var parsedValue))
                {
                    return TimeSpan.FromSeconds(parsedValue);
                }
            }

            return null;
        }
    }
}
