using DTO.Athletes;

namespace WEB.Services.Interfaces
{
    public interface IAthleteService
    {
        public Task<string> CheckAthlete();
        public Task<AddCoachResponseDTO> AddCoach();
        public Task<AthleteDetailsDTO> Details();
        public Task<AthleteDetailsDTO> MyDetails();
        public Task<AthleteDetailsDTO> EditDetails();

    }
}
