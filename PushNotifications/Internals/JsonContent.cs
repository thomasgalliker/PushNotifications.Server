using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace PushNotifications.Internals
{
    internal class JsonContent : StringContent
    {
        private const string JsonMediaType = "application/json";

        public JsonContent(object obj) : this(obj is string str ? str : JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))
        {

        }

        JsonContent(string content) : base(content, Encoding.UTF8, JsonMediaType)
        {
        }

        JsonContent(string content, Encoding encoding) : base(content, encoding, JsonMediaType)
        {
        }

        JsonContent(string content, Encoding encoding, string mediaType) : base(content, encoding, JsonMediaType)
        {
            
        }
    }
}