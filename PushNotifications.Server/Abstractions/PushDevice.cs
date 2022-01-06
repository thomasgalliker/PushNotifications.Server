using System.Diagnostics;

namespace PushNotifications.Server
{
    [DebuggerDisplay("PushDevice: {this.Platform}, DeviceToken={this.DeviceToken}")]
    public class PushDevice
    {
        public PushDevice(RuntimePlatform platform, string deviceToken)
        {
            this.Platform = platform;
            this.DeviceToken = deviceToken;
        }

        public static PushDevice Android(string registrationId)
        {
            return new PushDevice(RuntimePlatform.Android, registrationId);
        }

#pragma warning disable IDE1006 // Naming Styles
        public static PushDevice iOS(string token)
#pragma warning restore IDE1006 // Naming Styles
        {
            return new PushDevice(RuntimePlatform.iOS, token);
        }

        public RuntimePlatform Platform { get; private set; }

        public string DeviceToken { get; private set; }
    }
}