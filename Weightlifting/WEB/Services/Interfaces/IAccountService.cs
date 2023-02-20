using WEB.ViewModels.Athletes;

namespace WEB.Services
{
    public interface IAccountService
    {
        Task<string> CheckAccount();
        Task<string> RegisterAthlete(RegisterAthlete register);
    }
}