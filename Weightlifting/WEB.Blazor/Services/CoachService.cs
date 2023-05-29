using DTO.Athletes;
using DTO.Coaches;
using WEB.Blazor.Services.Interfaces;

namespace WEB.Blazor.Services
{
    public class CoachService : ICoachService
    {     
        public Task<string> Check()
        {
            throw new NotImplementedException();
        }

        public Task<AddAthleteToCoachResponseDTO> AddAthlete()
        {
            throw new NotImplementedException();
        }

        public Task<CoachDetailsDTO> Details()
        {
            throw new NotImplementedException();
        }

        public Task<CoachDetailsDTO> EditDetails()
        {
            throw new NotImplementedException();
        }

        public Task<CoachDetailsDTO> MyDetails()
        {
            throw new NotImplementedException();
        }
    }
}
