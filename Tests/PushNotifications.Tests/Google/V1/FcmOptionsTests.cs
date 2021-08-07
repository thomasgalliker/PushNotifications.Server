using System;
using FluentAssertions;
using PushNotifications.Google;
using Xunit;

namespace PushNotifications.Tests.Google.V1
{
    [Trait("Category", "UnitTests")]
    public class FcmOptionsTests
    {
        [Fact]
        public void ShouldThrowException_IfServiceAccountKeyFilePathIsNull()
        {
            // Act
            Action action = () => new FcmOptions
            {
                ServiceAccountKeyFilePath = null
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }
    }
}
