using PushNotifications.Messages;
using PushNotifications.Tests.Testdata;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace PushNotifications.Tests.Internals
{
    [Trait("Category", "UnitTests")]
    public class NotificationOverviewResultInternalTests
    {
        [Fact]
        public void ShouldDeserializeNotificationOverviewResultInternal()
        {
            // Arrange
            var json = NotificationOverviewResults.GetExample1_Json();

            // Act
            var notificationOverviewResultInternal = JsonConvert.DeserializeObject<NotificationOverviewResultInternal>(json);

            // Assert
            notificationOverviewResultInternal.Should().NotBeNull();
            notificationOverviewResultInternal.Values.Should().HaveCount(30);
        }
    }
}