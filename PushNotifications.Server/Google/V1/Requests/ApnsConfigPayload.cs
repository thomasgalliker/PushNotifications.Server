
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PushNotifications.Server.Google
{
    public class ApnsConfigPayload
    {
        [JsonProperty("aps")]
        public Aps Aps { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> CustomData { get; set; }
    }
}
