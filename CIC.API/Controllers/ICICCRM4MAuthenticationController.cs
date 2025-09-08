using CIC.API.DTO;
using CIC.API.DTO.RequestDTO;
using CIC.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace CIC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ICICCRM4MAuthenticationController : ControllerBase
    {
        private readonly ICICCRM4MAuthenticationService _mAuthenticationService;
        public PowerBISettings PowerBISettings { get; } = new PowerBISettings();
        public ICICCRM4MAuthenticationController(IConfiguration configuration, ICICCRM4MAuthenticationService mAuthenticationService)
        {
            _mAuthenticationService = mAuthenticationService;
            PowerBISettings = configuration.GetSection("PowerBISettings").Get<PowerBISettings>();
        }

        [HttpPost("/authenticateUser")]
        public async Task<IActionResult> Login(ICICCRM4MAuthenticationRequest request)
        {
            var result = await _mAuthenticationService.AuthenticateUserAsync(PowerBISettings.SecurityPassword,request);
            return Ok(result);
        }

        [HttpGet("/authenticateToken")]
        public async Task<IActionResult> AuthentivateToken(string token)
        {
            var result = await _mAuthenticationService.AuthenticateTokenAsync(PowerBISettings.SecurityPassword, token);
            return Ok(new ApiResponse() { Success = true, Result = result });
        }


        [HttpGet("/deleteUserSession")]
        public async Task<IActionResult> DeleteUserSession(string token)
        {            
            var result = await _mAuthenticationService.DeleteUserSession(PowerBISettings.SecurityPassword, token);
            return Ok(result);
        }
    }
}
