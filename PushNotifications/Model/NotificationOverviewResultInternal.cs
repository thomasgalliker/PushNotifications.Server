using System.Collections.Generic;
using Newtonsoft.Json;

namespace PushNotifications.Messages
{
    [JsonObject]
    internal class NotificationOverviewResultInternal
    {
        public NotificationOverviewResultInternal()
        {
            this.Values = new List<NotificationOverviewResult>();
        }

        [JsonProperty(PropertyName = "values")]
        public IEnumerable<NotificationOverviewResult> Values { get; set; }
    }
}