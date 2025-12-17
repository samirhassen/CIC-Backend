using Newtonsoft.Json;

namespace CIC.API.DTO.ResponseDTO
{
    public class AuthTokenResponse
    {
        public string status { get; set; }
        public string pa_webloginname { get; set; }
        [JsonProperty("emailaddress1")]
        public string Email { get; set; }
        public required string contactid { get; set; }
        public bool pa_member { get; set; }
        public required string pa_contactnumber { get; set; }
        public required string fullname { get; set; }
        public required string parentcustomerid_id { get; set; }
        public required string parentcustomerid { get; set; }
        public required string firstname { get; set; }
        public required string defaultpricelevelid_id { get; set; }
        public required string defaultpricelevelid { get; set; }
        public required string pa_labelname { get; set; }
        public required string lastname { get; set; }
        public required string pa_token { get; set; }
        public Account account { get; set; } = new Account();
        public required List<PaWebRole> pa_webroles { get; set; }
    }

    public class Account
    {
        public string cic_ipeds { get; set; }
    }   

}
