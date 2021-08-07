using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PushNotifications.Google;
using PushNotifications.Tests.Testdata;
using Xunit;

namespace PushNotifications.Tests.Apple
{
    [Trait("Category", "UnitTests")]
    public class FcmResponseTests
    {
        [Fact]
        public void ShouldCreateFcmResponse_Successful()
        {
            // Act
            var fcmResponse = FcmResponses.V1.GetFcmResponse_Success();

            // Assert
            fcmResponse.Should().NotBeNull();
            fcmResponse.IsSuccessful.Should().BeTrue();
            fcmResponse.Error.Should().BeNull();
            fcmResponse.Token.Should().Be("token");
        }

        [Fact]
        public void ShouldCreateFcmResponse_Error()
        {
            // Act
            var fcmResponse = FcmResponses.V1.GetFcmResponse_Error();

            // Assert
            fcmResponse.Should().NotBeNull();
            fcmResponse.IsSuccessful.Should().BeFalse();
            fcmResponse.Error.Should().NotBeNull();
            fcmResponse.Token.Should().Be("token");
        }

        [Theory]
        [ClassData(typeof(GetTokensWithRegistrationProblemTestdata))]
        public void ShouldGetTokensWithRegistrationProblem(string token, FcmErrorCode fcmErrorCode, ICollection<string> expectedTokensWithProblems)
        {
            // Arrange
            var fcmResponse = new FcmResponse
            {
                Token = token,
                Error = new FcmError
                {
                    Status = fcmErrorCode,
                },
            };

            // Act
            var tokensWithRegistrationProblems = fcmResponse.GetTokensWithRegistrationProblem();

            // Assert
            tokensWithRegistrationProblems.Should().HaveCount(expectedTokensWithProblems.Count());
            tokensWithRegistrationProblems.Should().ContainInOrder(expectedTokensWithProblems);
        }

        public class GetTokensWithRegistrationProblemTestdata : TheoryData<string, FcmErrorCode, ICollection<string>>
        {
            public GetTokensWithRegistrationProblemTestdata()
            {
                var token = new string('A', 64);

                // Positive
                this.Add(token, FcmErrorCode.Unregistered, new List<string> { token });

                // Negative
                this.Add(token, FcmErrorCode.QuotaExceeded, new List<string> { });
                this.Add(token, null, new List<string> { });
            }
        }
    }
}