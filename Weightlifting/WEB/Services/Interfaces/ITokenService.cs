namespace WEB.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetToken();
        Task RemoveToken();
        Task SetToken(string? token);
    }
}
