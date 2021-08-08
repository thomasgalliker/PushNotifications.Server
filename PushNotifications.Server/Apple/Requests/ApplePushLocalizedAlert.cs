using System;

using Newtonsoft.Json;

namespace PushNotifications.Server.Apple
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ApplePushLocalizedAlert
    {
        [JsonProperty("title-loc-key")]
        public string TitleLocKey { get; }
        
        [JsonProperty("title-loc-args")]
        public string[] TitleLocArgs { get; }
        
        [JsonProperty("loc-key")]
        public string LocKey { get; }
        
        [JsonProperty("loc-args")]
        public string[] LocArgs { get; }
        
        [JsonProperty("action-loc-key")]
        public string ActionLocKey { get; }

        public ApplePushLocalizedAlert(string locKey, string[] locArgs)
        {
            this.LocKey = locKey ?? throw new ArgumentNullException(nameof(locKey));
            this.LocArgs = locArgs ?? throw new ArgumentNullException(nameof(locArgs));
        }

        public ApplePushLocalizedAlert(string titleLocKey, string[] titleLocArgs, string locKey, string[] locArgs, string actionLocKey)
        {
            this.TitleLocKey = titleLocKey;
            this.TitleLocArgs = titleLocArgs;
            this.LocKey = locKey ?? throw new ArgumentNullException(nameof(locKey));;
            this.LocArgs = locArgs ?? throw new ArgumentNullException(nameof(locArgs));
            this.ActionLocKey = actionLocKey;
        }
    }

}
