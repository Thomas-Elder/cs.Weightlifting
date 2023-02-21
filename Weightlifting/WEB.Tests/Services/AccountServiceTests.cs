using Xunit;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Net.Http;
using WEB.Services;
using System.Net.Http.Json;
using WEB.ViewModels.Athletes;
using System.Threading.Tasks;

namespace WEB.Tests.Services
{ 
    public class AccountServiceTests
    {
        private readonly HttpClient _httpClient;
        private readonly HttpMessageHandler mock_handler;
        private readonly AccountService _sut;

        public AccountServiceTests()
        {
            mock_handler = Substitute.For<HttpMessageHandler>();
            _httpClient = new HttpClient(mock_handler);

            _sut = new AccountService(_httpClient);
        }

        //[Fact]
        public async void RegisterAthlete_WhenPassedInvalidRegistrationDetails_ReturnsFailureMessage()
        {
            // Arrange
            string expected = "Registration failed";

            RegisterAthlete input = new RegisterAthlete()
            {
                FirstName = "Fail",
                Email = "Bad email",
                Password = "Bad password"
            };

            HttpResponseMessage response = new HttpResponseMessage()
            {
                Content = new StringContent("test content"),
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };

            _httpClient.PostAsJsonAsync("Api url", Substitute.For<RegisterAthlete>())
                .Returns(Task.FromResult(response));

            // Act
            var actual = await _sut.RegisterAthlete(input);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
