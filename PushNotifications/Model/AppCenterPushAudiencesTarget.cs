using System.Collections.Generic;
using Newtonsoft.Json;

namespace PushNotifications.Messages
{
    public class AppCenterPushAudiencesTarget : AppCenterPushTarget
    {
        public override string Type => "audiences_target";

        [JsonProperty("audiences")]
        public IList<string> Audiences { get; set; } = new List<string>();
    }
}