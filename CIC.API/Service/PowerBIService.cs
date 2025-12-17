using CIC.API.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.PowerBI.Api.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CIC.API.Service
{
    public class PowerBIService : IPowerBIService
    {
        public PowerBISettings PowerBISetting { get; } = new PowerBISettings();
        private readonly HttpClient _httpClient;
        private readonly ILogger<PowerBIService> _logger;
        public PowerBIService(IConfiguration configuration, ILogger<PowerBIService> logger, HttpClient httpClient)
        {
            PowerBISetting = configuration.GetSection("PowerBISettings").Get<PowerBISettings>()
                ?? throw new InvalidOperationException("PowerBISettings configuration is missing.");
            _httpClient = httpClient;
            _logger = logger;
        }
        private async Task<string> AuthenticateAsync()
        {

            var authorityUri = $"{PowerBISetting.AuthorityUri}/{PowerBISetting.TenantId}/oauth2/v2.0/token";

            var form = new Dictionary<string, string>
            {
                {"client_id", PowerBISetting.ClientId },
                {"scope", PowerBISetting.Scope},
                {"client_secret", PowerBISetting.ClientSecret },
                {"grant_type", PowerBISetting.Granttype }
            };

            var response = await _httpClient.PostAsync(authorityUri, new FormUrlEncodedContent(form));
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Authentication failed: " + content);
            }

            var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            if (json == null || !json.TryGetValue("access_token", out var accessToken))
            {
                throw new Exception("Authentication failed: access_token missing.");
            }

            return accessToken;
        }


        public async Task<EmbeddedReportConfig> GetEmbedReportConfig(Guid reportId, string roleName, string email, string dataSetId)
        {
            Guid groupId = new Guid(PowerBISetting.GroupId);
            var embedUrl = "https://app.powerbi.com/reportEmbed?groupId=" + groupId + "&reportId=" + reportId;

            try
            {
                var accessToken = await AuthenticateAsync();
                var embedToken = await GenerateEmbedToken(accessToken, roleName, email, reportId, dataSetId);

                if (embedToken == null || string.IsNullOrWhiteSpace(embedToken.Token))
                {
                    _logger.LogError("Power BI embed token not generated for report {ReportId}", reportId);
                    throw new InvalidOperationException("Power BI embed token generation failed.");
                }

                return new EmbeddedReportConfig
                {
                    EmbedUrl = embedUrl,
                    GroupID = groupId.ToString(),
                    WebUrl = embedUrl,
                    ReportID = reportId.ToString(),
                    Token = embedToken.Token,
                    TokenID = embedToken.TokenId.ToString(),
                    Expiration = embedToken.Expiration,
                    IsExpired = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to build embed configuration for report {ReportId}", reportId);
                throw;
            }
        }

        public async Task<EmbedToken> GenerateEmbedToken(string token, string userName, string email, Guid reportId, string dataSetId)
        {
            string requestUrl = $"https://api.powerbi.com/v1.0/myorg/groups/{PowerBISetting.GroupId}/reports/{reportId}/GenerateToken";

            var effectiveUser = !string.IsNullOrWhiteSpace(userName) ? userName : email;
            if (string.IsNullOrWhiteSpace(effectiveUser))
            {
                _logger.LogWarning("Power BI GenerateToken called without effective user (email/userName) for report {ReportId}", reportId);
            }

            var requestBody = new
            {
                accessLevel = "View",
                identities = new[]
                  {
                        new
                        {
                            userName= userName,
                            roles=new []{ "InstitutionAccess" },
                            datasets=new []{ PowerBISetting.DataSets }
                        }
                    }
            };
            var jsonBody = System.Text.Json.JsonSerializer.Serialize(requestBody);

            if (reportId == new Guid(PowerBISetting.ReportIdAdminRole))
            {
                var requestBody1 = new
                {
                    accessLevel = "View",
                };
                jsonBody = System.Text.Json.JsonSerializer.Serialize(requestBody1);
            }



            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json")
            };

            httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            _logger.LogInformation("Requesting Power BI embed token for Report {ReportId}, Dataset {DatasetId}, EffectiveUser {User}", reportId, dataSetId, effectiveUser);

            var response = await _httpClient.SendAsync(httpRequest);
            string result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Power BI GenerateToken failed with status {StatusCode} for Report {ReportId}. Response: {Response}", response.StatusCode, reportId, result);
                throw new InvalidOperationException("Power BI GenerateToken request failed.");
            }

            if (!string.IsNullOrEmpty(result))
            {
                var embedToken = System.Text.Json.JsonSerializer.Deserialize<EmbedToken>(result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (embedToken == null)
                {
                    _logger.LogError("Power BI GenerateToken returned null payload for Report {ReportId}", reportId);
                    throw new InvalidOperationException("Power BI GenerateToken returned a null payload.");
                }
                return embedToken;
            }

            _logger.LogError("Power BI GenerateToken returned an empty payload for Report {ReportId}", reportId);
            throw new InvalidOperationException("Power BI GenerateToken returned an empty payload.");
        }
    }
}
