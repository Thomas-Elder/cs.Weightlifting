using DTO.Athletes;
using System.Net.Http.Headers;
using WEB.Services.Interfaces;

namespace WEB.Services
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
            // Get token
            var token = await _tokenService.GetToken();
            
            // Attach to request
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Send request
            var result = await _httpClient.GetFromJsonAsync<HttpResponse>("api/athlete/check");

            if (result is not null || result.StatusCode == 401)
            {
                return "failed to access";
            }

            return "success";
        }

        public Task<AthleteDetailsDTO> Details()
        {
            // Get token
            // Attach to request
            // Send request

            throw new NotImplementedException();
        }

        public Task<AthleteDetailsDTO> EditDetails()
        {
            throw new NotImplementedException();
        }

        public Task<AthleteDetailsDTO> MyDetails()
        {
            throw new NotImplementedException();
        }
    }
}
