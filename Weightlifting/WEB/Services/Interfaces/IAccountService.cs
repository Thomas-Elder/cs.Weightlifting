
using WEB.ViewModels.Account;

namespace WEB.Services
{
    public interface IAccountService
    {
        Task<string> CheckAccount();
        Task<string> RegisterAthlete(RegisterAthlete register);
        Task<string> Login(Login login);
    }
}