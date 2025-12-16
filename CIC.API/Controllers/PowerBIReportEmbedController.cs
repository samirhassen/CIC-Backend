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
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new { message = "Token is required." });
            }
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

                    const string adminRoleId = "2ac371cb-6583-f011-b4cc-000d3a1ebf95";
                    const string memberRoleId = "0cccc989-2842-e311-ac91-00155dfa7702";

                    var roles = user?.pa_webroles ?? new List<PaWebRole>();
                    bool hasAdminRole = roles.Any(r => string.Equals(r.Id, adminRoleId, StringComparison.OrdinalIgnoreCase));
                    bool hasMemberRole = roles.Any(r => string.Equals(r.Id, memberRoleId, StringComparison.OrdinalIgnoreCase));

                    var email = user?.emailaddress1 ?? string.Empty;

                    if (hasAdminRole)
                    {
                        reportId = PowerBISettings.ReportIdAdminRole;
                    }
                    else if (hasMemberRole)
                    {
                        reportId = PowerBISettings.ReportIdUserRole;
                    }
                    else
                    {
                        _logger.LogWarning("User {Email} missing required admin/member role for PowerBI embed", email);
                        return StatusCode(StatusCodes.Status403Forbidden, new { message = "Power BI report is unavailable for your account." });
                    }

                    if (string.IsNullOrWhiteSpace(reportId))
                    {
                        _logger.LogWarning("ReportId is missing for user {Email}", email);
                        return StatusCode(StatusCodes.Status502BadGateway, new { message = "Power BI report configuration is incomplete." });
                    }

                    try
                    {
                        embeddedReportConfig = await _iPowerBIService.GetEmbedReportConfig(new Guid(reportId), roleNames, email);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to retrieve embed config for user {Email}", email);
                        return StatusCode(StatusCodes.Status502BadGateway, new { message = "Unable to load Power BI report at this time." });
                    }

                    if (embeddedReportConfig == null || string.IsNullOrWhiteSpace(embeddedReportConfig.Token))
                    {
                        _logger.LogWarning("Embed token missing for user {Email}", email);
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
