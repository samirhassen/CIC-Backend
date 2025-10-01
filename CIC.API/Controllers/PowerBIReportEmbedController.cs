using CIC.API.DTO;
using CIC.API.DTO.ResponseDTO;
using CIC.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace CIC.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PowerBIReportEmbedController : ControllerBase
    {
        public PowerBISettings PowerBISettings { get; } = new PowerBISettings();
        private readonly IPowerBIService _iPowerBIService;
        public PowerBIReportEmbedController(IConfiguration configuration, ILogger<PowerBIReportEmbedController> logger, IPowerBIService powerBIService)
        {
            PowerBISettings = configuration.GetSection("PowerBISettings").Get<PowerBISettings>();
            _iPowerBIService = powerBIService;
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
                    var roleNames = user.account.cic_ipeds;               
                    EmbeddedReportConfig embeddedReportConfig = null;
                    string reportId = PowerBISettings.ReportId;
                    embeddedReportConfig = await _iPowerBIService.GetEmbedReportConfig(new Guid(reportId), roleNames, user.emailaddress1);
                    //Before sending config , apply companyid filter
                    embeddedReportConfig.EmbedUrl = string.Format("{0}&filter={1} eq {2}&filterPaneEnabled={3}", embeddedReportConfig.EmbedUrl, PowerBISettings.FilterName, "1", Convert.ToString(PowerBISettings.FilterPaneEnabled).ToLower());
                    embeddedReportConfig.ShowReport = true;
                    return Ok(embeddedReportConfig);
                }
                else
                {
                    return Ok("Token is not found");
                }
            }
            return Ok();            
        }       
    }
}
