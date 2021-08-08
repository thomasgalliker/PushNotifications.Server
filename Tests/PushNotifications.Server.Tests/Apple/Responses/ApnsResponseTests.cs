using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PushNotifications.Server.Apple;
using Xunit;

namespace PushNotifications.Server.Tests.Apple
{
    [Trait("Category", "UnitTests")]
    public class ApnsResponseTests
    {
        [Fact]
        public void ShouldCreateApnsResponse_Successful()
        {
            // Arrange
            var token = new string('X', 64);

            // Act
            var apnsResponse = new ApnsResponse(token);

            // Assert
            apnsResponse.Should().NotBeNull();
            apnsResponse.IsSuccessful.Should().BeTrue();
            apnsResponse.Reason.Should().BeNull();
            apnsResponse.Token.Should().Be(token);
        }

        [Fact]
        public void ShouldCreateApnsResponse_Error()
        {
            // Arrange
            var token = new string('X', 64);
            var reason = ApnsResponseReason.BadDeviceToken;

            // Act
            var apnsResponse = new ApnsResponse(token, reason);

            // Assert
            apnsResponse.Should().NotBeNull();
            apnsResponse.IsSuccessful.Should().BeFalse();
            apnsResponse.Reason.Should().Be(reason);
            apnsResponse.Token.Should().Be(token);
        }

        [Theory]
        [ClassData(typeof(GetTokensWithRegistrationProblemTestdata))]
        public void ShouldGetTokensWithRegistrationProblem(string token, ApnsResponseReason reason, ICollection<string> expectedTokensWithProblems)
        {
            // Arrange
            var apnsResponse = new ApnsResponse(token, reason);

            // Act
            var tokensWithRegistrationProblems = apnsResponse.GetTokensWithRegistrationProblem();

            // Assert
            tokensWithRegistrationProblems.Should().HaveCount(expectedTokensWithProblems.Count());
            tokensWithRegistrationProblems.Should().ContainInOrder(expectedTokensWithProblems);
        }

        public class GetTokensWithRegistrationProblemTestdata : TheoryData<string, ApnsResponseReason, ICollection<string>>
        {
            public GetTokensWithRegistrationProblemTestdata()
            {
                var token = new string('X', 64);

                // Positive
                this.Add(token, ApnsResponseReason.BadDeviceToken, new List<string> { token });
                this.Add(token, ApnsResponseReason.Unregistered, new List<string> { token });
                this.Add(token, ApnsResponseReason.MissingDeviceToken, new List<string> { token });

                // Negative
                this.Add(token, ApnsResponseReason.TooManyRequests, new List<string> { });
            }
        }
    }
}