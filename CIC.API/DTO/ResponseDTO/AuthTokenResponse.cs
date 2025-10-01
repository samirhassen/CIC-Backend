namespace CIC.API.DTO.ResponseDTO
{
    public class AuthTokenResponse
    {
        public string status { get; set; }
        public string pa_webloginname { get; set; }
        public string emailaddress1 { get; set; }
        public string contactid { get; set; }
        public bool pa_member { get; set; }
        public string pa_contactnumber { get; set; }
        public string fullname { get; set; }
        public string parentcustomerid_id { get; set; }
        public string parentcustomerid { get; set; }
        public string firstname { get; set; }
        public string defaultpricelevelid_id { get; set; }
        public string defaultpricelevelid { get; set; }
        public string pa_labelname { get; set; }
        public string lastname { get; set; }
        public string pa_token { get; set; }
        public Account account { get; set; }

        public List<PaWebRole> pa_webroles { get; set; }
    }

    public class Account
    {
        public string cic_ipeds { get; set; }
    }   

}
