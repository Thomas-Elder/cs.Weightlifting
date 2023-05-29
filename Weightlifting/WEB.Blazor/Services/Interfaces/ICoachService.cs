using DTO.Athletes;
using DTO.Coaches;

namespace WEB.Blazor.Services.Interfaces
{
    public interface ICoachService
    {
        public Task<string> Check();
        public Task<AddAthleteToCoachResponseDTO> AddAthlete();
        public Task<CoachDetailsDTO> Details();
        public Task<CoachDetailsDTO> MyDetails();
        public Task<CoachDetailsDTO> EditDetails();
    }
}
