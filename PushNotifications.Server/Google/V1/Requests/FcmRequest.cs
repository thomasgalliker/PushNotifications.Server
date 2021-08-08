using Newtonsoft.Json;

namespace PushNotifications.Server.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages/send
    /// </summary>
    public class FcmRequest : IPushRequest
    {
        [JsonProperty("validate_only")]
        public bool ValidateOnly { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }
    }
}
