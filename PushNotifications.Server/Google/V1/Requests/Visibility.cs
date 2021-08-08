using System;
using Newtonsoft.Json;

namespace PushNotifications.Server.Google
{
    /// <summary>
    /// Source: https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages#Visibility
    /// </summary>
    [JsonObject]
    public class Visibility : IEquatable<Visibility>
    {
        public static readonly Visibility Unknown = new Visibility();
        public static readonly Visibility Unspecified = new Visibility("VISIBILITY_UNSPECIFIED");
        public static readonly Visibility Private = new Visibility("PRIVATE");
        public static readonly Visibility Public = new Visibility("PUBLIC");
        public static readonly Visibility Secret = new Visibility("SECRET");

        private readonly string value;

        public Visibility(string value = null)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value;
        }

        public static implicit operator Visibility(string platform)
        {
            if (platform == null)
            {
                return null;
            }

            return new Visibility(platform);
        }

        public static implicit operator string(Visibility platform)
        {
            return platform.value;
        }

        public bool Equals(Visibility other)
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

            return this.Equals((Visibility)obj);
        }

        public override int GetHashCode()
        {
            return (this.value != null ? this.value.ToLowerInvariant().GetHashCode() : 0);
        }

        public static bool operator ==(Visibility left, Visibility right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Visibility left, Visibility right)
        {
            return !Equals(left, right);
        }
    }
}