using System.Text.Json.Serialization;

namespace CIC.API.DTO.RequestDTO
{
    public class LoginRequestDTO
    {
        [JsonPropertyName("client_id")]
        public string ClientId {  get; set; }=string.Empty;

        [JsonPropertyName("resource")]
        public string Resource {  get; set; }=string.Empty;

        [JsonPropertyName("client_secret")]
        public string ClientSecret {  get; set; }=string.Empty;

        //[JsonPropertyName("grant_type")]
        //public string GrantType {  get; set; }=string.Empty;
    }
}
