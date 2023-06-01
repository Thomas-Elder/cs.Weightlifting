using DTO.Athletes;

namespace WEB.Blazor.Services.Interfaces
{
    public interface IAthleteService
    {
        public Task<string> Check();
        public Task<AddCoachResponseDTO> AddCoach(int coachId);
        public Task<GetCoachesDTO> GetCoaches();
        public Task<AthleteDetailsDTO> Details();
        public Task<AthleteDetailsDTO> MyDetails();
        public Task<AthleteDetailsDTO> EditDetails(EditDetailsDTO editDetailsDTO);
    }
}
