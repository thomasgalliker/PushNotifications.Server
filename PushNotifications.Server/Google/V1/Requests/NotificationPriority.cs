using System;
using Newtonsoft.Json;

namespace PushNotifications.Server.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#NotificationPriority
    /// </summary>
    [JsonObject]
    public class NotificationPriority : IEquatable<NotificationPriority>
    {
        public static readonly NotificationPriority Unknown = new NotificationPriority();
        public static readonly NotificationPriority Unspecified = new NotificationPriority("PRIORITY_UNSPECIFIED");
        public static readonly NotificationPriority Min = new NotificationPriority("PRIORITY_MIN");
        public static readonly NotificationPriority Low = new NotificationPriority("PRIORITY_LOW");
        public static readonly NotificationPriority Default = new NotificationPriority("PRIORITY_DEFAULT");
        public static readonly NotificationPriority High = new NotificationPriority("PRIORITY_HIGH");
        public static readonly NotificationPriority Max = new NotificationPriority("PRIORITY_MAX");

        private readonly string value;

        public NotificationPriority(string value = null)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value;
        }

        public static implicit operator NotificationPriority(string platform)
        {
            if (platform == null)
            {
                return null;
            }

            return new NotificationPriority(platform);
        }

        public static implicit operator string(NotificationPriority platform)
        {
            return platform.value;
        }

        public bool Equals(NotificationPriority other)
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

            return this.Equals((NotificationPriority)obj);
        }

        public override int GetHashCode()
        {
            return (this.value != null ? this.value.ToLowerInvariant().GetHashCode() : 0);
        }

        public static bool operator ==(NotificationPriority left, NotificationPriority right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NotificationPriority left, NotificationPriority right)
        {
            return !Equals(left, right);
        }
    }
}