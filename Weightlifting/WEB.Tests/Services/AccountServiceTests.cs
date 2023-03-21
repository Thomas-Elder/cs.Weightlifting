using Xunit;

using NSubstitute;
using NSubstitute.ReturnsExtensions;

using System.Net;
using System.Threading;
using System;
using System.Net.Http;

using WEB.Services;
using WEB.ViewModels.Athletes;
using WEB.Tests.Utility;


namespace WEB.Tests.Services
{ 
    public class AccountServiceTests
    {
        [Fact]
        public async void RegisterAthlete_WhenPassedInvalidRegistrationDetails_ReturnsFailureMessage()
        {
            // Arrange
            string expected = "Registration failed";

            var mockHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("https://localhost:7207/") };

            var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            mockHttpMessageHandler.MockSend(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
               .Returns(mockResponse);

            AccountService _sut = new AccountService(httpClient);

            // Act
            var actual = await _sut.RegisterAthlete(Substitute.For<RegisterAthlete>());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void RegisterAthlete_WhenPassedValidRegistrationDetails_ReturnsSuccessMessage()
        {
            // Arrange
            string expected = "Registration success!";

            var mockHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("https://localhost:7207/") };

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);

            mockHttpMessageHandler.MockSend(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
               .Returns(mockResponse);

            AccountService _sut = new AccountService(httpClient);

            // Act
            var actual = await _sut.RegisterAthlete(Substitute.For<RegisterAthlete>());

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
