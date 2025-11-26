using CIC.API.Controllers;
using CIC.API.DTO;
using Microsoft.PowerBI.Api.Models;
using Microsoft.PowerBI.Api;
using Microsoft.Rest;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace CIC.API.Service
{
    public class PowerBIService : IPowerBIService
    {
        public PowerBISettings PowerBISetting { get; } = new PowerBISettings();
        private readonly HttpClient _httpClient;
        public PowerBIService(IConfiguration configuration, ILogger<PowerBIService> logger, HttpClient httpClient)
        {
            PowerBISetting = configuration.GetSection("PowerBISettings").Get<PowerBISettings>();
            _httpClient = httpClient;
        }
        private async Task<string> AuthenticateAsync()
        {

            var authorityUri = $"{PowerBISetting.AuthorityUri}/{PowerBISetting.TenantId}/oauth2/v2.0/token";

            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
        {
            {"client_id", PowerBISetting.ClientId },
            {"scope", PowerBISetting.Scope},
            {"client_secret", PowerBISetting.ClientSecret },
            {"grant_type", PowerBISetting.Granttype }
        };

                var response = await client.PostAsync(authorityUri, new FormUrlEncodedContent(form));
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Authentication failed: " + content);
                }

                var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                return json["access_token"];
            }
        }


        public async Task<EmbeddedReportConfig> GetEmbedReportConfig(Guid reportId, string roleName,string emailaddress1)
        {
            //Get Authentication Token from Azure
            var accessToken = await AuthenticateAsync();
            var embedToken = await GenerateEmbedToken(accessToken, roleName, emailaddress1, reportId);
            Guid groupId = new Guid(PowerBISetting.GroupId);
            EmbeddedReportConfig config = null;

            var embedUrl = "https://app.powerbi.com/reportEmbed?groupId=" + groupId + "&reportId=" + reportId;
            config = new EmbeddedReportConfig();
            config.EmbedUrl = embedUrl;
            config.GroupID = groupId.ToString(); // this we have in config
            config.WebUrl = embedUrl;  //// this we have in config "https://app.powerbi.com/reportEmbed?groupId=" + groupId + "&reportId=" + reportId;
            config.ReportID = reportId.ToString();
            config.Token = embedToken?.Token;
            config.TokenID = Convert.ToString(embedToken?.TokenId);
            config.Expiration = embedToken?.Expiration;
            config.IsExpired = false;
            return config;
        }

        public async Task<EmbedToken> GenerateEmbedToken(string token, string userName, string emailaddress1, Guid reportId)
        {
            string requestUrl = $"https://api.powerbi.com/v1.0/myorg/groups/{PowerBISetting.GroupId}/reports/{reportId}/GenerateToken";

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
            #region Dynamic Request
            //var requestBody = new
            //{
            //    accessLevel = "View",
            //    identities = new[]
            //    {
            //        new
            //        {
            //            userName= emailaddress1, //This logged in user email.
            //            roles=new []{"Member"},
            //            datasets=new []{ PowerBISetting.DataSets }
            //        }
            //    }
            //};
            #endregion Dynamic Request

            string jsonBody = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json")
            };

            httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(httpRequest);
            string result = await response.Content.ReadAsStringAsync();
            if(!string.IsNullOrEmpty(result))
            {
                var embedToken = System.Text.Json.JsonSerializer.Deserialize<EmbedToken>(result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return embedToken;
            }
            return null;            
        }
    }
}
