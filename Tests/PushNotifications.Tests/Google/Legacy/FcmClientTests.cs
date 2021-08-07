using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using PushNotifications.Google.Legacy;
using PushNotifications.Tests.Mocks;
using PushNotifications.Tests.Testdata;
using PushNotifications.Tests.Utils;
using Xunit;
using Xunit.Abstractions;
using FcmClient = PushNotifications.Google.Legacy.FcmClient;
using FcmOptions = PushNotifications.Google.Legacy.FcmOptions;

namespace PushNotifications.Tests.Google.Legacy
{
    [Trait("Category", "UnitTests")]
    public class FcmClientLegacyTests
    {
        private readonly TestOutputHelperLogger<FcmClient> logger;

        public FcmClientLegacyTests(ITestOutputHelper testOutputHelper)
        {
            this.logger = new TestOutputHelperLogger<FcmClient>(testOutputHelper);
        }

        [Fact]
        public async Task ShouldSendAsync_Succesful()
        {
            // Arrange
            var httpClientMock = new HttpClientMock();
            httpClientMock.SetupSendAsync()
                .ReturnsAsync(HttpResponseMessages.Success(JsonConvert.SerializeObject(FcmResponses.GetFcmResponse_Success())))
                .Verifiable();

            var fcmOptions = FcmTestOptions.GetFcmOptions();

            var fcmClient = new FcmClient(this.logger, httpClientMock.Object, fcmOptions);

            var registrationId = new string('A', 152);
            var fcmRequest = new FcmRequest()
            {
                RegistrationIds = new List<string> { registrationId },
                Notification = new FcmNotification
                {
                    Title = "title",
                    Body = "body",
                },
                Data = new Dictionary<string, string>
                {
                    { "key", "value" }
                },
            };

            // Act
            var fcmResponse = await fcmClient.SendAsync(fcmRequest);

            // Assert
            fcmResponse.Should().NotBeNull();
            fcmResponse.Results.Should().HaveCount(fcmRequest.RegistrationIds.Count);
            fcmResponse.Results.Select(r => r.RegistrationId).Should().ContainInOrder(fcmRequest.RegistrationIds);
            fcmResponse.NumberOfSuccesses.Should().Be(1);
            fcmResponse.NumberOfFailures.Should().Be(0);

            httpClientMock.VerifySendAsync(
                request => request.Method == HttpMethod.Post &&
                           request.RequestUri == new Uri("https://fcm.googleapis.com/fcm/send"),
                           Times.Exactly(1));

            httpClientMock.VerifyNoOtherCalls();
        }
    }
}