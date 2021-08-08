using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PushNotifications.Server.Internals
{
    internal class DurationStringToTimeSpanArrayJsonConverter : JsonConverter<TimeSpan[]>
    {
        public override TimeSpan[] ReadJson(JsonReader reader, Type objectType, TimeSpan[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                var timeSpans = token
                    .ToObject<IEnumerable<string>>()
                    .Select(s => DurationStringToTimeSpanJsonConverter.StringToTimeSpan(s))
                    .Where(t => t.HasValue)
                    .Select(t => t.Value)
                    .ToArray();

                return timeSpans;
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, TimeSpan[] values, JsonSerializer serializer)
        {
            if (values.Any())
            {
                writer.WriteStartArray();
                foreach (var value in values)
                {
                    string stringValue = DurationStringToTimeSpanJsonConverter.TimeSpanToString(value);
                    writer.WriteValue(stringValue);
                }
                writer.WriteEndArray();
            }
        }
    }
}
