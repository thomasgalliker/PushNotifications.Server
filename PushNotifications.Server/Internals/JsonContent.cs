using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace PushNotifications.Server.Internals
{
    internal class JsonContent : StringContent
    {
        private const string JsonMediaType = "application/json";

        public JsonContent(object obj) : this(obj is string str ? str : JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))
        {
        }

        public JsonContent(string content) : base(content, Encoding.UTF8, JsonMediaType)
        {
        }

        public JsonContent(string content, Encoding encoding) : base(content, encoding, JsonMediaType)
        {
        }
    }
}