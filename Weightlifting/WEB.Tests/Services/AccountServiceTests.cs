using Xunit;

using NSubstitute;

using System.Net;
using System.Threading;
using System;
using System.Net.Http;

using WEB.Services;
using WEB.Tests.Utility;

using DTO.Account;
using Microsoft.Extensions.Logging;
using WEB.Services.Interfaces;

namespace WEB.Tests.Services
{ 
    public class AccountServiceTests
    {
        [Fact]
        public async void RegisterAthlete_WhenPassedInvalidRegistrationDetails_ReturnsFailureMessage()
        {
            // Arrange
            var mockHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("https://localhost:7207/") };

            var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            mockHttpMessageHandler.MockSend(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
               .Returns(mockResponse);

            AccountService _sut = new AccountService(Substitute.For<ILogger<AccountService>>(), httpClient, Substitute.For<ITokenService>());

            // Act
            var actual = await _sut.RegisterAthlete(Substitute.For<UserRegistrationDTO>());

            // Assert
            Assert.False(actual.Success);
        }
        
        /*
        [Fact]
        public async void RegisterAthlete_WhenPassedValidRegistrationDetails_ReturnsSuccessMessage()
        {
            // Arrange
            var mockHttpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler) { BaseAddress = new Uri("https://localhost:7207/") };

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);

            mockHttpMessageHandler.MockSend(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
               .Returns(mockResponse);

            AccountService _sut = new AccountService(Substitute.For<ILogger<AccountService>>(), httpClient, Substitute.For<ITokenService>());

            // Act
            var actual = await _sut.RegisterAthlete(Substitute.For<UserRegistrationDTO>());

            // Assert
            Assert.True(actual.Success);
        }*/
    }
}
