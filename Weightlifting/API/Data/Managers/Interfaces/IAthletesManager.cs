using API.DTOs.Athletes;

namespace API.Data.Managers.Interfaces
{
    public interface IAthletesManager
    {
        public bool GetAthleteId(string applicationUserId, out int athleteId);
        public Task<AddCoachResponseDTO> AddCoach(int athleteId, int coachId);
        public Task<AthleteDetailsDTO> Details(int athleteId);
        public Task<AthleteDetailsDTO> EditDetails(int athleteId, EditDetailsDTO editDetailsDTO);
    }
}
