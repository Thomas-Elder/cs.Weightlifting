using WEB.Services.Interfaces;
using WEB.ViewModels.Account;

namespace WEB.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public AccountService(HttpClient httpClient,
            ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;

        }

        public async Task<string> CheckAccount()
        {
            var result = await _httpClient.GetFromJsonAsync<HttpResponse>("api/account/check");

            if (result is not null && result.StatusCode == 401)
            {
                return "failed to access";
            }

            return "success";
        }

        public async Task<string> RegisterAthlete(RegisterAthlete register)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account/register/athlete", register);

            if (!result.IsSuccessStatusCode)
            {
                return "Registration failed";
            }

            return "Registration success!";
        }

        public async Task<string> Login(Login login)
        {
            var result = await _httpClient.PostAsJsonAsync<UserAuthenticationResponseDTO>("api/account/login", login);

            if (!result.IsSuccessStatusCode)
            {
                return "Login failed";
            }

            await _tokenService.SetToken(result.Token);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
