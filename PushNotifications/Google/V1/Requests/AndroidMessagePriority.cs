using System;
using Newtonsoft.Json;

namespace PushNotifications.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#AndroidMessagePriority
    /// </summary>
    [JsonObject]
    public class AndroidMessagePriority : IEquatable<AndroidMessagePriority>
    {
        public static readonly AndroidMessagePriority Unknown = new AndroidMessagePriority();
        public static readonly AndroidMessagePriority Normal = new AndroidMessagePriority("NORMAL");
        public static readonly AndroidMessagePriority High = new AndroidMessagePriority("HIGH");

        private readonly string value;

        public AndroidMessagePriority(string value = null)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value;
        }

        public static implicit operator AndroidMessagePriority(string platform)
        {
            if (platform == null)
            {
                return null;
            }

            return new AndroidMessagePriority(platform);
        }

        public static implicit operator string(AndroidMessagePriority platform)
        {
            return platform.value;
        }

        public bool Equals(AndroidMessagePriority other)
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

            return this.Equals((AndroidMessagePriority)obj);
        }

        public override int GetHashCode()
        {
            return (this.value != null ? this.value.ToLowerInvariant().GetHashCode() : 0);
        }

        public static bool operator ==(AndroidMessagePriority left, AndroidMessagePriority right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AndroidMessagePriority left, AndroidMessagePriority right)
        {
            return !Equals(left, right);
        }
    }
}
