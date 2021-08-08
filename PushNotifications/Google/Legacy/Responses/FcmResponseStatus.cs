using System;
using Newtonsoft.Json;

namespace PushNotifications.Google.Legacy
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/cloud-messaging/http-server-ref#table9
    /// </summary>
    [JsonObject]
    public class FcmResponseStatus : IEquatable<FcmResponseStatus>
    {
        public static readonly FcmResponseStatus Unknown = new FcmResponseStatus();
        public static readonly FcmResponseStatus MissingRegistration = new FcmResponseStatus("MissingRegistration");
        public static readonly FcmResponseStatus InvalidRegistration = new FcmResponseStatus("InvalidRegistration");
        public static readonly FcmResponseStatus NotRegistered = new FcmResponseStatus("NotRegistered");
        public static readonly FcmResponseStatus InvalidPackageName = new FcmResponseStatus("InvalidPackageName");
        public static readonly FcmResponseStatus MismatchSenderId = new FcmResponseStatus("MismatchSenderId");
        public static readonly FcmResponseStatus InvalidParameters = new FcmResponseStatus("InvalidParameters");
        public static readonly FcmResponseStatus MessageTooBig = new FcmResponseStatus("MessageTooBig");
        public static readonly FcmResponseStatus InvalidDataKey = new FcmResponseStatus("InvalidDataKey");
        public static readonly FcmResponseStatus InvalidTtl = new FcmResponseStatus("InvalidTtl");
        public static readonly FcmResponseStatus Unavailable = new FcmResponseStatus("Unavailable");
        public static readonly FcmResponseStatus InternalServerError = new FcmResponseStatus("InternalServerError");
        public static readonly FcmResponseStatus DeviceMessageRate = new FcmResponseStatus("DeviceMessageRate");
        public static readonly FcmResponseStatus TopicsMessageRate = new FcmResponseStatus("TopicsMessageRate");
        public static readonly FcmResponseStatus InvalidApnsCredential = new FcmResponseStatus("InvalidApnsCredential");

        private readonly string value;

        internal FcmResponseStatus(string value = null)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value;
        }

        public static implicit operator FcmResponseStatus(string platform)
        {
            if (platform == null)
            {
                return null;
            }

            return new FcmResponseStatus(platform);
        }

        public static implicit operator string(FcmResponseStatus platform)
        {
            return platform.value;
        }

        public bool Equals(FcmResponseStatus other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.value, other.value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((FcmResponseStatus)obj);
        }

        public override int GetHashCode()
        {
            return this.value != null ? this.value.ToLowerInvariant().GetHashCode() : 0;
        }

        public static bool operator ==(FcmResponseStatus left, FcmResponseStatus right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FcmResponseStatus left, FcmResponseStatus right)
        {
            return !Equals(left, right);
        }
    }
}