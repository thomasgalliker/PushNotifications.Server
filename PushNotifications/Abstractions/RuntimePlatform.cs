using System;
using Newtonsoft.Json;

namespace PushNotifications
{
    [JsonObject]
    public class RuntimePlatform : IEquatable<RuntimePlatform>
    {
        public static readonly RuntimePlatform Android = new RuntimePlatform("Android");
        public static readonly RuntimePlatform iOS = new RuntimePlatform("iOS");
        public static readonly RuntimePlatform UWP = new RuntimePlatform("UWP");

        private readonly string value;

        public RuntimePlatform(string value = null)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value;
        }

        public static implicit operator RuntimePlatform(string platform)
        {
            if (platform == null)
            {
                return null;
            }

            return new RuntimePlatform(platform);
        }

        public static implicit operator string(RuntimePlatform platform)
        {
            return platform.value;
        }

        public bool Equals(RuntimePlatform other)
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

            return this.Equals((RuntimePlatform)obj);
        }

        public override int GetHashCode()
        {
            return (this.value != null ? this.value.ToLowerInvariant().GetHashCode() : 0);
        }

        public static bool operator ==(RuntimePlatform left, RuntimePlatform right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RuntimePlatform left, RuntimePlatform right)
        {
            return !Equals(left, right);
        }
    }
}