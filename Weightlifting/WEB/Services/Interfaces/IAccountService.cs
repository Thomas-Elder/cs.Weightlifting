
using DTO.Account;

namespace WEB.Services
{
    public interface IAccountService
    {
        Task<string> CheckAccount();
        Task<UserRegistrationResponseDTO> RegisterAthlete(UserRegistrationDTO register);
        Task<UserAuthenticationResponseDTO> Login(UserAuthenticationDTO login);
    }
}