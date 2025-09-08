using System.Text.Json.Serialization;

namespace CIC.API.DTO.ResponseDTO
{
    public class LoginResponseDTO
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public string ExpiresIn { get; set; } = string.Empty;

        [JsonPropertyName("ext_expires_in")]
        public string ExtExpiresIn { get; set; } = string.Empty;

        [JsonPropertyName("expires_on")]
        public string ExpiresOn { get; set; } = string.Empty;

        [JsonPropertyName("not_before")]
        public string NotBefore { get; set; } = string.Empty;

        [JsonPropertyName("resource")]
        public string Resource { get; set; } = string.Empty;

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
    }
}
