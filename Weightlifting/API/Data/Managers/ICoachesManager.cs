using API.Data.Models;
using API.DTOs.Coaches;

namespace API.Data.Managers
{
    public interface ICoachesManager
    {
        public Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(string coachUserId, int athleteId);
        public Task<CoachDetailsDTO> DetailsByApplicationUserId(string coachUserId);
        public Task<CoachDetailsDTO> DetailsByCoachId(int coachId);
        public Task<CoachDetailsDTO> EditDetailsByApplicationUserId(string coachUserId, EditDetailsDTO editDetailsDTO);
        public Task<CoachDetailsDTO> EditDetailsByCoachId(int coachId, EditDetailsDTO editDetailsDTO);
    }
}
