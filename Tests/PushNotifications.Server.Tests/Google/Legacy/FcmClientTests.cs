using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using PushNotifications.Server.Google.Legacy;
using PushNotifications.Server.Tests.Mocks;
using PushNotifications.Server.Tests.Testdata;
using PushNotifications.Server.Tests.Utils;
using Xunit;
using Xunit.Abstractions;

namespace PushNotifications.Server.Tests.Google.Legacy
{
    [Trait("Category", "UnitTests")]
    public class FcmClientTests
    {
        private readonly TestOutputHelperLogger<FcmClient> logger;

        public FcmClientTests(ITestOutputHelper testOutputHelper)
        {
            this.logger = new TestOutputHelperLogger<FcmClient>(testOutputHelper);
        }

        [Fact]
        public async Task ShouldSendAsync_Succesful()
        {
            // Arrange
            var httpClientMock = new HttpClientMock();
            httpClientMock.SetupSendAsync()
                .ReturnsAsync(HttpResponseMessages.Success(JsonConvert.SerializeObject(FcmResponses.Legacy.GetFcmResponse_Success())))
                .Verifiable();

            var fcmOptions = FcmTestOptions.Legacy.GetFcmOptions();

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

        [Fact]
        public async Task ShouldSendAsync_Failed()
        {
            // Arrange
            var httpClientMock = new HttpClientMock();
            httpClientMock.SetupSendAsync()
                .ReturnsAsync(HttpResponseMessages.BadRequest(JsonConvert.SerializeObject(FcmResponses.Legacy.GetFcmResponse_Error())))
                .Verifiable();

            var fcmOptions = FcmTestOptions.Legacy.GetFcmOptions();

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
            fcmResponse.NumberOfSuccesses.Should().Be(0);
            fcmResponse.NumberOfFailures.Should().Be(1);

            httpClientMock.VerifySendAsync(
                request => request.Method == HttpMethod.Post &&
                           request.RequestUri == new Uri("https://fcm.googleapis.com/fcm/send"),
                           Times.Exactly(1));

            httpClientMock.VerifyNoOtherCalls();
        }
    }
}