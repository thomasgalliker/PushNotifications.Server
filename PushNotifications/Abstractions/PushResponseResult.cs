using System.Diagnostics;

namespace PushNotifications
{
    [DebuggerDisplay("PushResponseResult: IsSuccessful={this.IsSuccessful}")]
    public class PushResponseResult
    {
        public bool IsSuccessful { get; set; }

        public string DeviceToken { get; set; }

        public IPushResponse OriginalResponse { get; set; }
    }
}