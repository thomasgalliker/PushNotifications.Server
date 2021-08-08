using System;
using Newtonsoft.Json;
using PushNotifications.Server.Internals;

namespace PushNotifications.Server.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#LightSettings
    /// </summary>
    [JsonObject]
    public class LightSettings
    {
        [JsonProperty("color")]
        public Color Color { get; set; }
        
        [JsonProperty("light_on_duration")]
        [JsonConverter(typeof(DurationStringToTimeSpanJsonConverter))]
        public TimeSpan? LightOnDuration { get; set; }

        [JsonProperty("light_off_duration")]
        [JsonConverter(typeof(DurationStringToTimeSpanJsonConverter))]
        public TimeSpan? LightOffDuration { get; set; }
    }
}