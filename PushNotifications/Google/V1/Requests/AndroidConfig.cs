using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PushNotifications.Internals;
using PushNotifications.Internals.JsonConverters;

namespace PushNotifications.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#AndroidConfig
    /// </summary>
    public class AndroidConfig
    {
        [JsonProperty("collapse_key")]
        public string CollapseKey { get; set; }

        [JsonProperty("priority")]
        [JsonConverter(typeof(AndroidMessagePriorityJsonConverter))]
        public AndroidMessagePriority Priority { get; set; }

        [JsonProperty("ttl")]
        [JsonConverter(typeof(DurationStringToTimeSpanJsonConverter))]
        public TimeSpan? TimeToLive { get; set; }

        [JsonProperty("restricted_package_name")]
        public string RestrictedPackageName { get; set; }

        [JsonProperty("data")]
        public IDictionary<string, string> Data { get; set; }

        [JsonProperty("notification")]
        public AndroidNotification Notification { get; set; }

        [JsonProperty("fcm_options")]
        public AndroidFcmOptions FcmOptions { get; set; }

        [JsonProperty("direct_boot_ok")]
        public bool DirectBootOk { get; set; }
    }
}
