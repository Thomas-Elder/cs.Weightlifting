using WEB.Services.Interfaces;

using DTO.Account;

namespace WEB.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public AccountService(
            ILogger<AccountService> logger,
            HttpClient httpClient,
            ITokenService tokenService)
        {
            _logger = logger;
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

        public async Task<UserRegistrationResponseDTO> RegisterAthlete(UserRegistrationDTO register)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/account/register/athlete", register);
                var result = await response.Content.ReadFromJsonAsync<UserRegistrationResponseDTO>() ?? throw new Exception("Invalid response from server.");

                return result;

            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new UserRegistrationResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> { "There was an error registering your account, please try again shortly." }
                };
            }
        }

        public async Task<UserAuthenticationResponseDTO> Login(UserAuthenticationDTO login)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/account/login", login);
                var result = await response.Content.ReadFromJsonAsync<UserAuthenticationResponseDTO>() ?? throw new Exception("Invalid response from server.");

                await _tokenService.SetToken(result.Token);

                return result;

            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new UserAuthenticationResponseDTO()
                {
                    Success = false,
                    Errors = new List<string> { "There was an error logging you in, please try again shortly." }
                };
            }            
        }
    }
}
