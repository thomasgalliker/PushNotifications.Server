using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PushNotifications.Messages
{
    [JsonObject]
    [DebuggerDisplay("AppCenterPushContent: {this.Name}")]
    public class AppCenterPushContent
    {
        /// <summary>
        ///     Campaign Name field with a descriptive name for the campaign.
        ///     The value you provide will display in the App Center campaign list page.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///     (optional) Populate the Title field with an optional title for the notification sent to target devices.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        ///     Populate the Message field with the content for the notification message.
        ///     Message content is limited to 4,000 characters.
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        ///     Use the Custom data area of the form to define up to 20 key/value data pairs that you want included with the
        ///     message. The maximum number of characters per custom data value is 100.
        /// </summary>
        [JsonProperty("custom_data")]
        public Dictionary<string, string> CustomData { get; set; } = new Dictionary<string, string>();
    }
}