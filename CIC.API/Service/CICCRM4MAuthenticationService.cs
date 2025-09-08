using CIC.API.DTO.RequestDTO;
using CIC.API.DTO.ResponseDTO;
using CRM4MServiceReference;
using System.Text.Json;
namespace CIC.API.Service
{
    public class CICCRM4MAuthenticationService : ICICCRM4MAuthenticationService
    {        

        public async Task<AuthUserResponse> AuthenticateUserAsync(string securityPassword,ICICCRM4MAuthenticationRequest request)
        {
            CRM4MServiceReference.AuthenticationWebServiceSoapClient client = new(CRM4MServiceReference.AuthenticationWebServiceSoapClient.EndpointConfiguration.Authentication_x0020_Web_x0020_ServiceSoap);
            var response = await client.AuthenticateUserAsync(securityPassword, request.Username, request.Password);           

            var user = JsonSerializer.Deserialize<AuthUserResponse>(response.Body.AuthenticateUserResult);
            return user;
        }

        public async Task<AuthTokenResponse> AuthenticateTokenAsync(string securityPassword,string token)
        {
            CRM4MServiceReference.AuthenticationWebServiceSoapClient client = new(CRM4MServiceReference.AuthenticationWebServiceSoapClient.EndpointConfiguration.Authentication_x0020_Web_x0020_ServiceSoap);
            var response = await client.AuthenticateTokenAsync( securityPassword, token);
            var user = JsonSerializer.Deserialize<AuthTokenResponse>(response.Body.AuthenticateTokenResult);
            return user;

        }

        public async Task<DeleteSessionResponse> DeleteUserSession(string securityPassword, string token)
        {
            CRM4MServiceReference.AuthenticationWebServiceSoapClient client = new(CRM4MServiceReference.AuthenticationWebServiceSoapClient.EndpointConfiguration.Authentication_x0020_Web_x0020_ServiceSoap);
            var response = await client.DeleteUserSessionAsync(securityPassword, token);
            var user = JsonSerializer.Deserialize<DeleteSessionResponse>(response.Body.DeleteUserSessionResult);
            return user;            
        }
    }
}
