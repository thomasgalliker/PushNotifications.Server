using Newtonsoft.Json;

namespace PushNotifications.Server.Google
{
    public class Color
    {
        [JsonProperty("red")]
        public float Red { get; set; }

        [JsonProperty("green")]
        public float Green { get; set; }

        [JsonProperty("blue")]
        public float Blue { get; set; }

        [JsonProperty("alpha")]
        public float Alpha { get; set; }
    }
}