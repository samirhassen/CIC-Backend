namespace CIC.API.DTO
{
    public class EmbeddedReportConfig
    {
        public string GroupID { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string TokenID { get; set; } = string.Empty;
        public DateTime? Expiration { get; set; } 
        public string ReportID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string WebUrl { get; set; } = string.Empty;
        public string EmbedUrl { get; set; } = string.Empty;
        public string DatasetId { get; set; } = string.Empty;
        public bool IsExpired { get; set; }
        public bool ShowReport { get; set; } = false;
    }
}
