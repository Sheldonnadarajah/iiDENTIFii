using Microsoft.AspNetCore.Mvc;
using AspireApp1.ApiService.Application.Auth;
using AspireApp1.ApiService.Domain.Interfaces;
using AspireApp1.ApiService.Domain.Entities;
using System.Threading.Tasks;

namespace AspireApp1.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var (success, tokenOrError) = await _authService.LoginAsync(request);
            if (!success)
                return Unauthorized(tokenOrError);
            return Ok(new { token = tokenOrError });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand request)
        {
            var (success, error) = await _authService.RegisterAsync(request);
            if (!success)
            {
                if (error == "Username already exists.")
                    return Conflict(error);
                return BadRequest(error);
            }
            return Ok(error);
        }
    }
}
