using API.DTOs.Coaches;

namespace API.Data.Managers
{
    public interface IAthletesManager
    {
        public Task<AddAthleteToCoachResponseDTO> AddAthleteToCoach(string athleteUserId, int coachId);
    }
}
