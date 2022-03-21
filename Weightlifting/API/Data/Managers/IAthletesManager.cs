using API.DTOs.Athletes;

namespace API.Data.Managers
{
    public interface IAthletesManager
    {
        public bool GetAthleteId(string applicationUserId, out int athleteId);
        public bool UserIsAthlete(string applicationUserId, int athleteId);

        public Task<AddCoachResponseDTO> AddCoach(string athleteUserId, int coachId);

        public Task<AthleteDetailsDTO> Details(int athleteId);
        public Task<AthleteDetailsDTO> EditDetails(int athleteId, EditDetailsDTO editDetailsDTO);
    }
}
