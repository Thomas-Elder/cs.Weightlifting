using DTO.Athletes;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WEB.Blazor.Services.Interfaces;

namespace WEB.Blazor.Services
{
    public class AthleteService : IAthleteService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public AthleteService(
            ILogger<AccountService> logger,
            HttpClient httpClient,
            ITokenService tokenService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _tokenService = tokenService;

        }

        public Task<AddCoachResponseDTO> AddCoach()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Check()
        {
            var token = await _tokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.GetAsync("athlete/check");

            if (result is null)
            {
                return "Failed to access as an athlete. The result of the call to the server was null.";
            }

            if (!result.IsSuccessStatusCode)
            {
                return $"Failed to successfully access as an athlete. The result has the status {result.StatusCode}";
            }

            return await result.Content.ReadAsStringAsync();
        }

        public async Task<AthleteDetailsDTO> Details()
        {
            throw new NotImplementedException();
        }

        public Task<AthleteDetailsDTO> EditDetails()
        {
            throw new NotImplementedException();
        }

        public async Task<AthleteDetailsDTO> MyDetails()
        {
            var token = await _tokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.GetFromJsonAsync<AthleteDetailsDTO>("Athlete/MyDetails");

            if (result is null)
            {
                return new AthleteDetailsDTO()
                {
                    Errors = new List<string>() { "Failed to access the api. The result of the call to the server was null." }
                };
            }

            if (!result.Success)
            {
                return new AthleteDetailsDTO()
                {
                    Errors = result.Errors
                };
            }

            return result;
        }
    }
}
