using System.Net.Http.Headers;

namespace PushNotifications.Internals
{
    internal static class HttpClientUtils
    {
        public static ProductInfoHeaderValue GetProductInfo<T>(T source)
        {
            var type = source.GetType();
            return new ProductInfoHeaderValue($"PushNotifications.{(type.Name)}", type.Assembly.GetName().Version.ToString());
        }
    }
}
