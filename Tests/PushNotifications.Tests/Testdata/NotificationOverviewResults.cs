using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using PushNotifications.Messages;
using Newtonsoft.Json;

namespace PushNotifications.Tests.Testdata
{
    internal static class NotificationOverviewResults
    {
        public static HttpResponseMessage Success(string notificationId, int count)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new NotificationOverviewResultInternal
                {
                    Values = GenerateNotificationOverviewResults(notificationId, count).ToList()
                }))
            };
        }

        private static IEnumerable<NotificationOverviewResult> GenerateNotificationOverviewResults(string notificationId, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new NotificationOverviewResult
                {
                    NotificationId = $"{notificationId}_{i}",
                    Name = $"name_{i}",
                    PnsSendFailure = 1,
                    PnsSendSuccess = 2,
                    SendTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    State = NotificationState.Completed
                };
            }
        }

        public static string GetExample1_Json()
        {
            var json = ResourceLoader.Current.GetEmbeddedResourceString(typeof(NotificationOverviewResults).Assembly, "NotificationOverviewResults_Example1.json");
            return json;
        }

        public static NotificationOverviewResultInternal GetExample1()
        {
            return new NotificationOverviewResultInternal
            {
                Values = new List<NotificationOverviewResult>
                {
                    new NotificationOverviewResult
                    {
                        NotificationId = "notification_id_test_0",
                        Name = "name_0",
                        NotificationTarget = null,
                        SendTime = DateTime.ParseExact("2000-01-01T00:00:00.0000000Z", "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                        PnsSendFailure = 1,
                        PnsSendSuccess = 2,
                        State = NotificationState.Completed,
                        RuntimePlatform = RuntimePlatform.Android
                    },
                    new NotificationOverviewResult
                    {
                        NotificationId = "notification_id_test_1",
                        Name = "name_1",
                        NotificationTarget = null,
                        SendTime = DateTime.ParseExact("2000-01-01T00:00:00.0000000Z", "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                        PnsSendFailure = 1,
                        PnsSendSuccess = 2,
                        State = NotificationState.Completed,
                        RuntimePlatform = RuntimePlatform.Android
                    },
                    new NotificationOverviewResult
                    {
                        NotificationId = "notification_id_test_2",
                        Name = "name_2",
                        NotificationTarget = null,
                        SendTime = DateTime.ParseExact("2000-01-01T00:00:00.0000000Z", "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                        PnsSendFailure = 1,
                        PnsSendSuccess = 2,
                        State = NotificationState.Completed,
                        RuntimePlatform = RuntimePlatform.Android
                    },
                    new NotificationOverviewResult
                    {
                        NotificationId = "notification_id_test_0",
                        Name = "name_0",
                        NotificationTarget = null,
                        SendTime = DateTime.ParseExact("2000-01-01T00:00:00.0000000Z", "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                        PnsSendFailure = 1,
                        PnsSendSuccess = 2,
                         State = NotificationState.Completed,
                        RuntimePlatform = RuntimePlatform.iOS
                    },
                    new NotificationOverviewResult
                    {
                        NotificationId = "notification_id_test_1",
                        Name = "name_1",
                        NotificationTarget = null,
                        SendTime = DateTime.ParseExact("2000-01-01T00:00:00.0000000Z", "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                        PnsSendFailure = 1,
                        PnsSendSuccess = 2,
                        State = NotificationState.Completed,
                        RuntimePlatform = RuntimePlatform.iOS
                    },
                    new NotificationOverviewResult
                    {
                        NotificationId = "notification_id_test_2",
                        Name = "name_2",
                        NotificationTarget = null,
                        SendTime = DateTime.ParseExact("2000-01-01T00:00:00.0000000Z", "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                        PnsSendFailure = 1,
                        PnsSendSuccess = 2,
                        State = NotificationState.Completed,
                        RuntimePlatform = RuntimePlatform.iOS
                    }
                }
            };
        }
    }
}
