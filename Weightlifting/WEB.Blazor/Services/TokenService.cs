﻿using Blazored.LocalStorage;
using WEB.Blazor.Services.Interfaces;

namespace WEB.Blazor.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILocalStorageService localStorageService;

        public TokenService(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public async Task SetToken(string? token)
        {
            await localStorageService.SetItemAsync("token", token);
        }

        public async Task<string> GetToken()
        {
            return await localStorageService.GetItemAsync<string>("token");
        }

        public async Task RemoveToken()
        {
            await localStorageService.RemoveItemAsync("token");
        }
    }
}
