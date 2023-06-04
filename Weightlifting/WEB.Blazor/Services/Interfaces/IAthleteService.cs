using DTO.Athletes;
using DTO.Sessions;

namespace WEB.Blazor.Services.Interfaces
{
    public interface IAthleteService
    {
        public Task<string> Check();

        // Coach
        public Task<AddCoachResponseDTO> AddCoach(int coachId);
        public Task<GetCoachesDTO> GetCoaches();

        // Details
        public Task<AthleteDetailsDTO> Details();
        public Task<AthleteDetailsDTO> MyDetails();
        public Task<AthleteDetailsDTO> EditDetails(EditDetailsDTO editDetailsDTO);

        // Session methods
        public Task<AddSessionResponseDTO> AddSession(AddSessionDTO sessionDTO);
        public Task<GetSessionsResponseDTO> GetSessions(int athleteId);
    }
}
