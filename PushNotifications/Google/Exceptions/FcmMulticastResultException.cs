using System;
using System.Collections.Generic;

namespace PushNotifications.Google
{
    public class FcmMulticastResultException : Exception
    {
        public FcmMulticastResultException () : base ("One or more Registration Id's failed in the multicast notification")
        {
            this.Succeeded = new List<FcmRequest> ();
            this.Failed = new Dictionary<FcmRequest, Exception> ();
        }

        public List<FcmRequest> Succeeded { get;set; }

        public Dictionary<FcmRequest, Exception> Failed { get;set; }
    }
}

