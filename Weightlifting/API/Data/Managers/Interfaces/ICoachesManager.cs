using API.DTOs.Coaches;

namespace API.Data.Managers.Interfaces
{
    public interface ICoachesManager
    {
        public bool GetCoachId(string applicationUserId, out int coachId);
        public Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(string coachUserId, int athleteId);
        public Task<CoachDetailsResponseDTO> Details(int coachId);
        public Task<CoachDetailsResponseDTO> EditDetails(int coachId, EditDetailsDTO editDetailsDTO);
    }
}
