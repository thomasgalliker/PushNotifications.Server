﻿using System;
using Newtonsoft.Json;
using PushNotifications.Server.Google;

namespace PushNotifications.Server.Internals.JsonConverters
{
    internal class FcmErrorCodeJsonConverter : JsonConverter<FcmErrorCode>
    {
        public override FcmErrorCode ReadJson(JsonReader reader, Type objectType, FcmErrorCode existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string stringValue)
            {
                return new FcmErrorCode(stringValue);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, FcmErrorCode value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
