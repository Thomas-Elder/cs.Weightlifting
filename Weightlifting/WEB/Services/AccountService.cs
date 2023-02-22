using WEB.ViewModels.Athletes;

namespace WEB.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;

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
    }
}
