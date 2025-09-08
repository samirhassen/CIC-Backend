using System.Text.Json.Serialization;

namespace CIC.API.DTO.ResponseDTO
{
    public class DeleteSessionResponse
    {
        [JsonPropertyName("status")]
        public string Status {  get; set; }=string.Empty;
    }
}
