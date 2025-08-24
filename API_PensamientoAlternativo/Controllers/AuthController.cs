using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PensamientoAlternativo.Application.Commands;
using static Google.Apis.Requests.RequestError;

namespace API_PensamientoAlternativo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator Mediator;
        public AuthController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var cmd = new LoginCommand { Email = request.Email, Password = request.Password };
            var res = await Mediator.Send(cmd);

            if (res is null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(res);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(claims);
        }
    }
}
