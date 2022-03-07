using API.DTOs.Athletes;

namespace API.Data.Managers
{
    public interface IAthletesManager
    {
        public Task<AddCoachResponseDTO> AddCoach(string athleteUserId, int coachId);
        public Task<AddSessionResponseDTO> AddSession(string athleteUserId, AddSessionDTO addSessionDTO);
    }
}
