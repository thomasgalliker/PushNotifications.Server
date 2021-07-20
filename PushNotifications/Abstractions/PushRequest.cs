using System.Collections.Generic;

namespace PushNotifications
{
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