using Microsoft.Identity.Client;

namespace CIC.API.DTO
{
    public class AppSettings
    {
        public string AuthorityUri { get; set; } = string.Empty;
        public string ResourceUrl { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;
        public string ApiUrl { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string ApplicationId { get; set; } = string.Empty;
        public string LoggingRequestUrl { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public string ReportId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string Granttype { get; set; } = string.Empty;
        public string TokenType { get; set; } = string.Empty;
        public string FilterName { get; set; } = string.Empty;
        public string FilterPaneEnabled { get; set; } = string.Empty;
    }

    public class PowerBISettings
    {
        
        public string AuthorityUri { get; set; } = string.Empty;
        public string ReportId { get; set; } = string.Empty;
        public string TenantId {get; set; } = string.Empty;

        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;
        public string Granttype { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;

        // Embedded Token Settings
        public string GroupId { get; set; } = string.Empty;
        public string ApiUrl { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string DataSets { get; set; } = string.Empty;
        public string SecurityPassword {  get; set; } = string.Empty;
        public string FilterName { get; set; } = string.Empty;
        public bool FilterPaneEnabled {  get; set; } = false;
    }
}
