using System.Diagnostics;

namespace PushNotifications
{
    [DebuggerDisplay("PushResponseResult: IsSuccessful={this.IsSuccessful}")]
    public class PushResponseResult
    {
        public PushResponseResult(IPushResponse originalResponse, string deviceToken, bool isSuccessful)
        {
            this.OriginalResponse = originalResponse ?? throw new System.ArgumentNullException(nameof(originalResponse));
            this.IsSuccessful = isSuccessful;
            this.DeviceToken = deviceToken ?? throw new System.ArgumentNullException(nameof(deviceToken));
        }

        public IPushResponse OriginalResponse { get; }

        public bool IsSuccessful { get; }

        public string DeviceToken { get; }
    }
}