using System.Collections.Generic;

namespace PushNotifications
{
    /// <summary>
    /// Cross-platform abstraction of a push request.
    /// </summary>
    public class PushRequest : IPushRequest
    {
        public PushRequest()
        {
            this.Devices = new List<PushDevice>();
        }

        public ICollection<PushDevice> Devices { get; set; }

        public PushContent Content { get; set; }
    }
}