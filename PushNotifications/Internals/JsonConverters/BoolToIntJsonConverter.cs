using System;
using Newtonsoft.Json;

namespace PushNotifications.Internals
{
    internal class BoolToIntJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            bool booleanValue = (bool)value;

            writer.WriteValue(Convert.ToInt32(booleanValue));
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Convert.ToBoolean(reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(bool) == objectType;
        }
    }
}
