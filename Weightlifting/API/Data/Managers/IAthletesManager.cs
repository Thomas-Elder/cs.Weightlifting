using API.DTOs.Coaches;

namespace API.Data.Managers
{
    public interface IAthletesManager
    {
        public Task<AddCoachResponseDTO> AddCoach(string athleteUserId, int coachId);
    }
}
