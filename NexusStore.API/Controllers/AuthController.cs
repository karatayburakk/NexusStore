using Microsoft.AspNetCore.Mvc;
using NexusStore.API.Services;
using NexusStore.API.Dtos;

namespace NexusStore.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController(IAuthService authService) : ControllerBase
  {
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
      var token = await _authService.LoginAsync(loginDto);
      if (token == null) return Unauthorized();
      return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
      var user = await _authService.RegisterAsync(registerDto);
      if (user == null) return BadRequest("User registration failed.");
      return Ok(user);
    }
  }
}
