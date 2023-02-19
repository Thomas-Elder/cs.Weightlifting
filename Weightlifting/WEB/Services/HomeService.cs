﻿namespace WEB.Services
{
    public class HomeService : IHomeService
    {
        private readonly HttpClient _httpClient;

        public HomeService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<string> CheckConnection()
        {
            HttpResponseMessage result = await _httpClient.GetAsync("api/home/check");

            if (!result.IsSuccessStatusCode)
            {
                return "Connection failure";
            }

            return await result.Content.ReadAsStringAsync();
        }
    }
}
