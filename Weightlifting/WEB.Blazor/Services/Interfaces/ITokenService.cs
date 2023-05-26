namespace WEB.Blazor.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetToken();
        Task RemoveToken();
        Task SetToken(string? token);
    }
}
