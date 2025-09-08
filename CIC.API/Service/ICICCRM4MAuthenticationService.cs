using CIC.API.DTO.RequestDTO;
using CIC.API.DTO.ResponseDTO;
using CRM4MServiceReference;

namespace CIC.API.Service
{
    public interface ICICCRM4MAuthenticationService
    {
        Task<AuthUserResponse> AuthenticateUserAsync(string securityPassword,ICICCRM4MAuthenticationRequest request);
        Task<AuthTokenResponse> AuthenticateTokenAsync(string securityPassword, string token);
        Task<DeleteSessionResponse> DeleteUserSession(string securityPassword, string token);
    }
}
