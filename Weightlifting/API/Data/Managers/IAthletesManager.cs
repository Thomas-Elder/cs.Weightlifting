using API.DTOs.Athletes;

namespace API.Data.Managers
{
    public interface IAthletesManager
    {
        public bool GetAthleteId(string applicationUserId, out int athleteId);

        public Task<AddCoachResponseDTO> AddCoach(string athleteUserId, int coachId);

        public Task<AthleteDetailsDTO> DetailsByApplicationUserId(string athleteUserId);
        public Task<AthleteDetailsDTO> DetailsByAthleteId(int athleteId);
        public Task<AthleteDetailsDTO> EditDetailsByApplicationUserId(string athleteUserId, EditDetailsDTO editDetailsDTO);
        public Task<AthleteDetailsDTO> EditDetailsByAthleteId(int athleteId, EditDetailsDTO editDetailsDTO);

        //public Task<SessionDetailsDTO> GetSessionDetails(int athleteId);
        public Task<AddSessionResponseDTO> AddSession(string athleteUserId, AddSessionDTO addSessionDTO);
    }
}
