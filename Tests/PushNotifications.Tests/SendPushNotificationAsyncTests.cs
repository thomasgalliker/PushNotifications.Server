using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Blauhaus.Push.Server.AppCenter.Service;
using Blauhaus.Push.Tests.Mocks;
using Blauhaus.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using PushNotifications.Model;
using Blauhaus.Common.Config.AppCenter.Server;
using Blauhaus.Common.ValueObjects.RuntimePlatforms;
using Blauhaus.HttpClientService.Request;
using Blauhaus.Push.Common.Notifications;
using Blauhaus.Push.Server.AppCenter.Dtos;

namespace Blauhaus.Push.Tests.Tests.Server.AppCenter.AppCenterPushNotificationServerServiceTests
{
    public class SendPushNotificationAsyncTests : BaseUnitTest<AppCenterPushNotificationServerService>
    {
        private HttpClientServiceMockBuilder _mockHttpClientService;


        [SetUp]
        public void OnSetup()
        {
            Cleanup();
            _mockHttpClientService = new HttpClientServiceMockBuilder();
        }

        protected override AppCenterPushNotificationServerService ConstructSut()
        {
            return new AppCenterPushNotificationServerService(
                _mockHttpClientService.Object,
                Mock.Of<ILogger<AppCenterPushNotificationServerService>>());
        }

        private class TestConfig : BaseAppCenterServerConfig
        {
            public TestConfig() : base("organizationName", "apiToken")
            {
                AppNames[RuntimePlatform.Android] = "androidAppName";
                AppNames[RuntimePlatform.iOS] = "iosAppName";
                AppNames[RuntimePlatform.UWP] = "uwpAppName";
            }
        }


        [Test]
        public async Task SHOULD_call_http_client_using_correct_endpoint_and_appname_and_Device_Ids_for_each_target_platform()
        {

            //Arrange
            var notification = new PushNotification
            {
                Title = "Test",
                DeviceTargets = new List<PushNotificationTarget>
                {
                    new PushNotificationTarget{TargetDevicePlatform = RuntimePlatform.UWP, TargetDeviceId = "1"},
                    new PushNotificationTarget{TargetDevicePlatform = RuntimePlatform.iOS, TargetDeviceId = "2"},
                    new PushNotificationTarget{TargetDevicePlatform = RuntimePlatform.iOS, TargetDeviceId = "3"},
                }
            };


            //Act
            await Sut.SendPushNotificationAsync(notification, new TestConfig());

            //Assert
            _mockHttpClientService.Mock.Verify(x => x.PostAsync<AppCenterPushRequestDto, AppCenterPushResponseDto>(It.Is<IHttpRequestWrapper<AppCenterPushRequestDto>>(y => 
                y.Request.NotificationTarget.Devices.FirstOrDefault(a => a == "1") != null &&
                y.Request.NotificationTarget.Devices.FirstOrDefault(b => b == "2") == null &&
                y.Request.NotificationTarget.Devices.FirstOrDefault(c => c == "3") == null &&
                y.Endpoint == "https://api.appcenter.ms/v0.1/apps/organizationName/uwpAppName/push/notifications" &&
                y.RequestHeaders["X-API-Token"] == "apiToken"), CancellationToken.None));
            _mockHttpClientService.Mock.Verify(x => x.PostAsync<AppCenterPushRequestDto, AppCenterPushResponseDto>(It.Is<IHttpRequestWrapper<AppCenterPushRequestDto>>(y => 
                y.Request.NotificationTarget.Devices.FirstOrDefault(a => a == "1") == null &&
                y.Request.NotificationTarget.Devices.FirstOrDefault(b => b == "2") != null &&
                y.Request.NotificationTarget.Devices.FirstOrDefault(c => c == "3") != null &&
                y.Endpoint == "https://api.appcenter.ms/v0.1/apps/organizationName/iosAppName/push/notifications" &&
                y.RequestHeaders["X-API-Token"] == "apiToken"), CancellationToken.None));
        }

        [Test]
        public async Task SHOULD_convert_push_notification_to_DTO()
        {

            //Arrange
            var notification = new PushNotification
            {
                Name = "Name",
                Title = "Title",
                Body = "Body",
                DeviceTargets = new List<PushNotificationTarget>
                {
                    new PushNotificationTarget{TargetDevicePlatform = RuntimePlatform.UWP, TargetDeviceId = "1"},
                }
            };

            //Act
            await Sut.SendPushNotificationAsync(notification, new TestConfig());

            //Assert
            _mockHttpClientService.Mock.Verify(x => x.PostAsync<AppCenterPushRequestDto, AppCenterPushResponseDto>(It.Is<IHttpRequestWrapper<AppCenterPushRequestDto>>(y => 
                y.Request.NotificationTarget.Type == "devices_target"  &&
                y.Request.NotificationContent.Name == "Name" &&
                y.Request.NotificationContent.Title == "Title" &&
                y.Request.NotificationContent.Body == "Body"), CancellationToken.None));
        }

        [Test]
        public async Task SHOULD_add_custom_data()
        {

            //Arrange
            var notification = new PushNotification
            {
                Title = "Test",
                Body = "Body",
                DeviceTargets = new List<PushNotificationTarget>
                {
                    new PushNotificationTarget{TargetDevicePlatform = RuntimePlatform.UWP, TargetDeviceId = "1"},
                },
                NotificationType = "Type",
                Sound = "default",
                BadgeCount = 12,
                TargetId = "123",
            };
            notification.CustomData["FavouriteColour"] = "Red";

            //Act
            await Sut.SendPushNotificationAsync(notification, new TestConfig());

            //Assert
            _mockHttpClientService.Mock.Verify(x => x.PostAsync<AppCenterPushRequestDto, AppCenterPushResponseDto>(It.Is<IHttpRequestWrapper<AppCenterPushRequestDto>>(y => 
                y.Request.NotificationContent.CustomData["sound"] == "default" &&
                y.Request.NotificationContent.CustomData["badgeCount"] == "12" &&
                y.Request.NotificationContent.CustomData["notificationType"] == "Type" &&
                y.Request.NotificationContent.CustomData["targetId"] == "123" &&
                y.Request.NotificationContent.CustomData["FavouriteColour"] == "Red"
            ), CancellationToken.None));
        }
    }
}