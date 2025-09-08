using CIC.API.DTO.RequestDTO;
using CIC.API.DTO.ResponseDTO;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CIC.API.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _iconfiguration;
        public LoginRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _iconfiguration = configuration;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO login)
        {
            var tenantId = _iconfiguration.GetValue<string>("AzureAd:TenantId");
            var url = $"https://login.microsoftonline.com/{tenantId} /oauth2/token";

            var request = new Dictionary<string, string>
            {
                ["client_id"] = login.ClientId,
                ["client_secret"] = login.ClientSecret,
                ["resource"] = login.Resource,
                ["grant_type"] = "client_credentials"
            };
            using var content = new FormUrlEncodedContent(request);
            var response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<LoginResponseDTO>(responseBody);
                return tokenResponse;
            }
            else
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return null;
            }
           
        }
    }


}
