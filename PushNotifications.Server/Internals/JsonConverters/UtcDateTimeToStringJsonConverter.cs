using System;
using System.Globalization;
using Newtonsoft.Json;

namespace PushNotifications.Server.Internals
{
    internal class UtcDateTimeToStringJsonConverter : JsonConverter<DateTime?>
    {
        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFZ";

        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string stringValue && DateTime.TryParseExact(stringValue, DefaultDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dateTimeValue))
            {
                return dateTimeValue;
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            if (value is DateTime dateTimeValue)
            {
                writer.WriteValue(dateTimeValue.ToString(DefaultDateTimeFormat));
            }
        }
    }
}