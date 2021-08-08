using System;
using Newtonsoft.Json;

namespace PushNotifications.Google
{
    /// <summary>
    /// Error codes for FCM failure conditions.
    /// 
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/ErrorCode
    /// </summary>
    [JsonObject]
    public class FcmErrorCode : IEquatable<FcmErrorCode>
    {
        public static readonly FcmErrorCode Unknown = new FcmErrorCode();
        public static readonly FcmErrorCode UnspecifiedError = new FcmErrorCode("UNSPECIFIED_ERROR");
        public static readonly FcmErrorCode InvalidArgument = new FcmErrorCode("INVALID_ARGUMENT");
        public static readonly FcmErrorCode Unregistered = new FcmErrorCode("UNREGISTERED");
        public static readonly FcmErrorCode SenderIdMismatch = new FcmErrorCode("SENDER_ID_MISMATCH");
        public static readonly FcmErrorCode QuotaExceeded = new FcmErrorCode("QUOTA_EXCEEDED");
        public static readonly FcmErrorCode Unavailable = new FcmErrorCode("UNAVAILABLE");
        public static readonly FcmErrorCode Internal = new FcmErrorCode("INTERNAL");
        public static readonly FcmErrorCode ThirdPartyAuthError = new FcmErrorCode("THIRD_PARTY_AUTH_ERROR");

        private readonly string value;

        internal FcmErrorCode(string value = null)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value;
        }

        public static implicit operator FcmErrorCode(string platform)
        {
            if (platform == null)
            {
                return null;
            }

            return new FcmErrorCode(platform);
        }

        public static implicit operator string(FcmErrorCode platform)
        {
            return platform.value;
        }

        public bool Equals(FcmErrorCode other)
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

            return this.Equals((FcmErrorCode)obj);
        }

        public override int GetHashCode()
        {
            return this.value != null ? this.value.ToLowerInvariant().GetHashCode() : 0;
        }

        public static bool operator ==(FcmErrorCode left, FcmErrorCode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FcmErrorCode left, FcmErrorCode right)
        {
            return !Equals(left, right);
        }
    }
}