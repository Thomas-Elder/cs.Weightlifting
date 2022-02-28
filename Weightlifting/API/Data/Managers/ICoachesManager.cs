using API.Data.Models;
using API.DTOs.Coaches;

namespace API.Data.Managers
{
    public interface ICoachesManager
    {
        public Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(string coachUserId, int athleteId);
        public Task<GetAthletesResponseDTO> GetAthletes(string id);
    }
}
