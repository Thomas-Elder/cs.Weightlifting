
using DTO.Athletes;
using DTO.Sessions;

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

        public async Task<AddCoachResponseDTO> AddCoach(int coachId)
        {
            var token = await _tokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Athlete/Coach/Add/" + coachId);
            var result = await response.Content.ReadFromJsonAsync<AddCoachResponseDTO>();

            if (result is null)
            {
                return new AddCoachResponseDTO()
                {
                    Errors = new List<string>() { "Failed to access the api. The result of the call to the server was null." }
                };
            }

            return result;
        }

        public async Task<GetCoachesDTO> GetCoaches()
        {
            var token = await _tokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Athlete/Coach/Get");
            var result = await response.Content.ReadFromJsonAsync<GetCoachesDTO>();

            if (result is null)
            {
                return new GetCoachesDTO()
                {
                    Errors = new List<string>() { "Failed to access the api. The result of the call to the server was null." }
                };
            }

            return result;
        }

        public Task<AthleteDetailsDTO> Details()
        {
            throw new NotImplementedException();
        }

        public async Task<AthleteDetailsDTO> EditDetails(EditDetailsDTO editDetailsDTO)
        {
            var token = await _tokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("Athlete/Details/Edit", editDetailsDTO);
            var result = await response.Content.ReadFromJsonAsync<AthleteDetailsDTO>();

            if (result is null)
            {
                return new AthleteDetailsDTO()
                {
                    Errors = new List<string>() { "Failed to access the api. The result of the call to the server was null." }
                };
            }

            return result;
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

            return result;
        }

        public async Task<GetSessionsResponseDTO> GetSessions()
        {
            var token = await _tokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await _httpClient.GetFromJsonAsync<GetSessionsResponseDTO>("Sessions/AthleteSessions");

            var response = await _httpClient.GetAsync("Sessions/AthleteSessions");
            var result = await response.Content.ReadFromJsonAsync<GetSessionsResponseDTO>();

            if (result is null)
            {
                return new GetSessionsResponseDTO()
                {
                    Errors = new List<string>() { "Failed to access the api. The result of the call to the server was null." }
                };
            }

            return result;
        }

        public async Task<AddSessionResponseDTO> AddSession(AddSessionDTO sessionDTO)
        {
            var token = await _tokenService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("Sessions/Add", sessionDTO);
            var result = await response.Content.ReadFromJsonAsync<AddSessionResponseDTO>();

            if (result is null)
            {
                return new AddSessionResponseDTO()
                {
                    Errors = new List<string>() { "Failed to access the api. The result of the call to the server was null." }
                };
            }

            return result;
        }
    }
}
