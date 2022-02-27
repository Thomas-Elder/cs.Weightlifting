using API.Data.Models;
using API.Account.DTOs;

namespace API.Data.Managers
{
    public class AccountManager : IAccountManager
    {
        public Task<ApplicationUser> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UserAuthenticationResponseDTO> Login(UserAuthenticationDTO userAuthenticationDTO)
        {
            throw new NotImplementedException();
        }

        public Task<UserRegistrationResponseDTO> Register(UserRegistrationDTO userRegistrationDTO)
        {
            throw new NotImplementedException();
        }
    }
}
