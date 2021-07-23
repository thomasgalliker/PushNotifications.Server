using FluentAssertions;
using Xunit;

namespace PushNotifications.Tests
{
    [Trait("Category", "UnitTests")]
    public class RuntimePlatformTests
    {
        [Fact]
        public void ShouldCreateRuntimePlatformFromString()
        {
            // Act
            var runtimePlatform = (RuntimePlatform)"android";

            // Assert
            runtimePlatform.Should().Be(RuntimePlatform.Android);
        }
        
        [Fact]
        public void ShouldCreateRuntimePlatformFromString_Implicit()
        {
            // Act
            RuntimePlatform runtimePlatform = "android";

            // Assert
            runtimePlatform.Should().Be(RuntimePlatform.Android);
        }

        [Fact]
        public void ShouldBeEqualLowercaseAndUppercase()
        {
            var runtimePlatformLowercase = (RuntimePlatform)"ios";
            var runtimePlatformUppercase = (RuntimePlatform)"IOS";

            // Act
            var equals = runtimePlatformLowercase == runtimePlatformUppercase;

            // Assert
            equals.Should().BeTrue();
        }
    }
}
