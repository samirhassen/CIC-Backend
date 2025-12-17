using CIC.API.DTO;
using CIC.API.DTO.ResponseDTO;
using CIC.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CIC.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PowerBIReportEmbedController : ControllerBase
    {
        public PowerBISettings PowerBISettings { get; } = new PowerBISettings();
        private readonly IPowerBIService _iPowerBIService;
        private readonly ILogger<PowerBIReportEmbedController> _logger;
        public PowerBIReportEmbedController(IConfiguration configuration, ILogger<PowerBIReportEmbedController> logger, IPowerBIService powerBIService)
        {
            PowerBISettings = configuration.GetSection("PowerBISettings").Get<PowerBISettings>()
                ?? throw new InvalidOperationException("PowerBISettings configuration is missing.");
            _iPowerBIService = powerBIService;
            _logger = logger;
        }

        [Route("getReport")]
        [HttpGet]
        public async Task<IActionResult> GetReport(string token)
        {
            //if token valid then only execute below code
            CRM4MServiceReference.AuthenticationWebServiceSoapClient client = new(CRM4MServiceReference.AuthenticationWebServiceSoapClient.EndpointConfiguration.Authentication_x0020_Web_x0020_ServiceSoap);
            var tokenResponse = await client.AuthenticateTokenAsync(PowerBISettings.SecurityPassword, token);
            if (tokenResponse != null)
            {
                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthTokenResponse>(tokenResponse.Body.AuthenticateTokenResult);
                if (user?.pa_token != null)
                {
                    var roleNames = user?.account?.cic_ipeds ?? string.Empty;
                    EmbeddedReportConfig? embeddedReportConfig = null;
                    string reportId = PowerBISettings.ReportIdUserRole;
                    string dataSetId = PowerBISettings.DataSets;

                    var roles = user?.pa_webroles ?? new List<PaWebRole>();
                    bool hasAdminRole = roles.Any(r => string.Equals(r.Id, PowerBISettings.AdminRoleId, StringComparison.OrdinalIgnoreCase));
                    bool hasMemberRole = roles.Any(r => string.Equals(r.Id, PowerBISettings.MemberRoleId, StringComparison.OrdinalIgnoreCase));

                    if (hasAdminRole)
                    {
                        reportId = PowerBISettings.ReportIdAdminRole;
                        dataSetId = PowerBISettings.DataSetsAdmin;
                    }
                    else if (hasMemberRole)
                    {
                        reportId = PowerBISettings.ReportIdUserRole;
                        dataSetId = PowerBISettings.DataSets;
                    }
                    else
                    {
                        _logger.LogWarning("User {Email} missing required admin/member role for PowerBI embed", user?.Email);
                        return StatusCode(StatusCodes.Status403Forbidden, new { message = "Power BI report is unavailable for your account." });
                    }

                    if (string.IsNullOrWhiteSpace(reportId))
                    {
                        _logger.LogWarning("ReportId is missing for user {Email}", user?.Email);
                        return StatusCode(StatusCodes.Status502BadGateway, new { message = "Power BI report configuration is incomplete." });
                    }

                    try
                    {
                        embeddedReportConfig = await _iPowerBIService.GetEmbedReportConfig(new Guid(reportId), roleNames, user?.Email, dataSetId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve embed config for user {Email}", user?.Email);
                        return StatusCode(StatusCodes.Status502BadGateway, new { message = "Unable to load Power BI report at this time." });
                    }

                    if (embeddedReportConfig == null || string.IsNullOrWhiteSpace(embeddedReportConfig.Token))
                    {
                        _logger.LogWarning("Embed token missing for user {Email}", user?.Email);
                        return StatusCode(StatusCodes.Status502BadGateway, new { message = "Power BI report is temporarily unavailable." });
                    }

                    embeddedReportConfig.EmbedUrl = string.Format("{0}&filter={1} eq {2}&filterPaneEnabled={3}&pageName={4}", embeddedReportConfig.EmbedUrl, PowerBISettings.FilterName, "1", Convert.ToString(PowerBISettings.FilterPaneEnabled).ToLower(),"1. Home");
                    embeddedReportConfig.ShowReport = true;
                    return Ok(embeddedReportConfig);
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Authentication failed for the provided token." });
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Authentication failed for the provided token." });
        }
    }
}
