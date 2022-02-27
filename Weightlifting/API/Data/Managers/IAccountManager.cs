using API.Data.Models;
using API.DTOs.Account;

namespace API.Data.Managers
{
    public interface IAccountManager
    {
        public Task<UserRegistrationResponseDTO> Register(UserRegistrationDTO userRegistrationDTO);
        public Task<UserAuthenticationResponseDTO> Login(UserAuthenticationDTO userAuthenticationDTO);
        public Task<ApplicationUser> Get(string id);
    }
}
