using DTO.Athletes;
using DTO.Coaches;
using System.Net.Http.Headers;
using WEB.Blazor.Services.Interfaces;

namespace WEB.Blazor.Services
{
    public class CoachService : ICoachService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public CoachService(
            ILogger<AccountService> logger,
            HttpClient httpClient,
            ITokenService tokenService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _tokenService = tokenService;

        }

        public async Task<string> Check()
        {
            var token = await _tokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var result = await _httpClient.GetAsync("coach/check");

            if (result is null)
            {
                return "Failed to access as an coach. The result of the call to the server was null.";
            }

            if (!result.IsSuccessStatusCode)
            {
                return $"Failed to successfully access as an coach. The result has the status {result.StatusCode}";
            }

            return await result.Content.ReadAsStringAsync();
        }

        public async Task<AddAthleteToCoachResponseDTO> AddAthlete()
        {
            throw new NotImplementedException();
        }

        public async Task<CoachDetailsDTO> Details()
        {
            throw new NotImplementedException();
        }

        public async Task<CoachDetailsDTO> EditDetails()
        {
            throw new NotImplementedException();
        }

        public async Task<CoachDetailsDTO> MyDetails()
        {
            throw new NotImplementedException();
        }
    }
}
