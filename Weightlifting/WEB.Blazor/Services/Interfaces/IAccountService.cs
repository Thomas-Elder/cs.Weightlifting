
using DTO.Account;

namespace WEB.Blazor.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string> CheckAccount();
        Task<UserRegistrationResponseDTO> RegisterAthlete(UserRegistrationDTO register);
        Task<UserRegistrationResponseDTO> RegisterCoach(UserRegistrationDTO register);
        Task<UserAuthenticationResponseDTO> Login(UserAuthenticationDTO login);
        Task Logout();
    }
}