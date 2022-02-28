using API.Data.Models;
using API.DTOs.Account;

namespace API.Data.Managers
{
    public interface IAccountManager
    {
        public Task<UserRegistrationResponseDTO> RegisterCoach(UserRegistrationDTO userRegistrationDTO);
        public Task<UserRegistrationResponseDTO> RegisterAthlete(UserRegistrationDTO userRegistrationDTO);
        public Task<UserAuthenticationResponseDTO> Login(UserAuthenticationDTO userAuthenticationDTO);
    }
}
