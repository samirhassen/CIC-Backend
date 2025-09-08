
using CIC.API.DTO;
using CIC.API.DTO.RequestDTO;
using CIC.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace CIC.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        //[HttpPost("/api/Login")]
        //public async Task<IActionResult> UpdateStatusByModuleId([FromBody] LoginRequestDTO login)
        //{
        //    var loginObj = await _loginService.LoginAsync(login);
        //    if (loginObj == null)
        //    {
        //        return Ok(new ApiResponse() { Success = false, Errors = new List<string>() { "Getting error while login,Please try again...!" } });
        //    }
        //    else
        //    {
        //        return Ok(new ApiResponse() { Success = true, Result = loginObj });
        //    }
        //}
    }
}
