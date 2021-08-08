
using System;
using Newtonsoft.Json;

namespace PushNotifications.Server.Apple
{
    //[JsonConverter(typeof(ApnsResponseReasonJsonConverter))]
    public class ApnsResponseReason : IEquatable<ApnsResponseReason>
    {
        public static readonly ApnsResponseReason Unknown = new ApnsResponseReason();

        // 400
        /// <summary>
        /// The collapse identifier exceeds the maximum allowed size.
        /// </summary>
        public static readonly ApnsResponseReason BadCollapseId = new ApnsResponseReason("BadCollapseId");

        /// <summary>
        /// The specified device token is invalid. Verify that the request contains a valid token and that the token matches the environment.
        /// </summary>
        public static readonly ApnsResponseReason BadDeviceToken = new ApnsResponseReason("BadDeviceToken");

        /// <summary>
        /// The apns-expiration value is invalid.
        /// </summary>
        public static readonly ApnsResponseReason BadExpirationDate = new ApnsResponseReason("BadExpirationDate");
        public static readonly ApnsResponseReason BadMessageId = new ApnsResponseReason("BadMessageId");
        public static readonly ApnsResponseReason BadPriority = new ApnsResponseReason("BadPriority");
        public static readonly ApnsResponseReason BadTopic = new ApnsResponseReason("BadTopic");
        public static readonly ApnsResponseReason DeviceTokenNotForTopic = new ApnsResponseReason("DeviceTokenNotForTopic");
        public static readonly ApnsResponseReason DuplicateHeaders = new ApnsResponseReason("DuplicateHeaders");
        public static readonly ApnsResponseReason IdleTimeout = new ApnsResponseReason("IdleTimeout");
        public static readonly ApnsResponseReason InvalidPushType = new ApnsResponseReason("InvalidPushType");
        public static readonly ApnsResponseReason MissingDeviceToken = new ApnsResponseReason("MissingDeviceToken");
        public static readonly ApnsResponseReason MissingTopic = new ApnsResponseReason("MissingTopic");
        public static readonly ApnsResponseReason PayloadEmpty = new ApnsResponseReason("PayloadEmpty");
        public static readonly ApnsResponseReason TopicDisallowed = new ApnsResponseReason("TopicDisallowed");

        // 403
        public static readonly ApnsResponseReason BadCertificate = new ApnsResponseReason("BadCertificate");
        public static readonly ApnsResponseReason BadCertificateEnvironment = new ApnsResponseReason("BadCertificateEnvironment");
        public static readonly ApnsResponseReason ExpiredProviderToken = new ApnsResponseReason("ExpiredProviderToken");
        public static readonly ApnsResponseReason Forbidden = new ApnsResponseReason("Forbidden");
        public static readonly ApnsResponseReason InvalidProviderToken = new ApnsResponseReason("InvalidProviderToken");
        public static readonly ApnsResponseReason MissingProviderToken = new ApnsResponseReason("MissingProviderToken");

        // 404
        public static readonly ApnsResponseReason BadPath = new ApnsResponseReason("BadPath");

        // 405
        public static readonly ApnsResponseReason MethodNotAllowed = new ApnsResponseReason("MethodNotAllowed");

        // 410
        public static readonly ApnsResponseReason Unregistered = new ApnsResponseReason("Unregistered");

        // 413
        public static readonly ApnsResponseReason PayloadTooLarge = new ApnsResponseReason("PayloadTooLarge");

        // 429
        public static readonly ApnsResponseReason TooManyProviderTokenUpdates = new ApnsResponseReason("TooManyProviderTokenUpdates");
        public static readonly ApnsResponseReason TooManyRequests = new ApnsResponseReason("TooManyRequests");

        // 500
        public static readonly ApnsResponseReason InternalServerError = new ApnsResponseReason("InternalServerError");

        // 503
        public static readonly ApnsResponseReason ServiceUnavailable = new ApnsResponseReason("ServiceUnavailable");
        public static readonly ApnsResponseReason Shutdown = new ApnsResponseReason("Shutdown");
 
        private readonly string value;

        [JsonConstructor]
        internal ApnsResponseReason(string value = null)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value;
        }

        public static implicit operator ApnsResponseReason(string platform)
        {
            if (platform == null)
            {
                return null;
            }

            return new ApnsResponseReason(platform);
        }

        public static implicit operator string(ApnsResponseReason platform)
        {
            return platform.value;
        }

        public bool Equals(ApnsResponseReason other)
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

            return this.Equals((ApnsResponseReason)obj);
        }

        public override int GetHashCode()
        {
            return (this.value != null ? this.value.ToLowerInvariant().GetHashCode() : 0);
        }

        public static bool operator ==(ApnsResponseReason left, ApnsResponseReason right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ApnsResponseReason left, ApnsResponseReason right)
        {
            return !Equals(left, right);
        }
    }
}