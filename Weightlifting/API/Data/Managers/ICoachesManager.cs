using API.DTOs.Coaches;

namespace API.Data.Managers
{
    public interface ICoachesManager
    {
        public Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(string coachUserId, int athleteId);
        public Task<CoachDetailsResponseDTO> DetailsByApplicationUserId(string coachUserId);
        public Task<CoachDetailsResponseDTO> DetailsByCoachId(int coachId);
        public Task<CoachDetailsResponseDTO> EditDetailsByApplicationUserId(string coachUserId, EditDetailsDTO editDetailsDTO);
        public Task<CoachDetailsResponseDTO> EditDetailsByCoachId(int coachId, EditDetailsDTO editDetailsDTO);
    }
}
