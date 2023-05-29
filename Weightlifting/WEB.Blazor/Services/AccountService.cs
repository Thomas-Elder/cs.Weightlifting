using WEB.Blazor.Services.Interfaces;

using DTO.Account;
using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace WEB.Blazor.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        private readonly AuthenticationStateService _myAuthenticationStateProvider;

        public AccountService(
            ILogger<AccountService> logger,
            HttpClient httpClient,
            ITokenService tokenService,
            AuthenticationStateService myAuthenticationStateProvider)
        {
            _logger = logger;
            _httpClient = httpClient;
            _tokenService = tokenService;
            _myAuthenticationStateProvider = myAuthenticationStateProvider; 
        }

        public async Task<string> CheckAccount()
        {
            var token = await _tokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.GetAsync("api/account/check");

            if (result is null)
            {
                return "Failed to access";
            }

            if (!result.IsSuccessStatusCode)
            {
                return $"Failed to successfully access authorized endpoint. The result has the status {result.StatusCode}";
            }

            return await result.Content.ReadAsStringAsync();
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

        public async Task<UserRegistrationResponseDTO> RegisterCoach(UserRegistrationDTO register)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/account/register/coach", register);
                var result = await response.Content.ReadFromJsonAsync<UserRegistrationResponseDTO>() ?? throw new Exception("Invalid response from server.");

                return result;

            }
            catch (Exception ex)
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

                _myAuthenticationStateProvider.StateChanged();

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

        public async Task Logout()
        {
            await _tokenService.RemoveToken();

            _myAuthenticationStateProvider.StateChanged();
        }
    }
}
