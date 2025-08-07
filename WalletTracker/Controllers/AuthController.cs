using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using WalletTracker.Dtos;
using WalletTracker.Hasher;

namespace WalletTracker.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        await _authService.Register(registerDto.Username, registerDto.Email, registerDto.Password);
        return Ok();
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var token = await _authService.Login(loginDto.Email, loginDto.Password);

            Response.Cookies.Append("auth-token", token, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
            return Ok(new { Message = "Authentication successful" });
        }
        catch (AuthenticationException e)
        {
            return Unauthorized(new { e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during login");
            return StatusCode(500, new { Message = "Internal server error" });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("auth-token", new CookieOptions()
        {
            HttpOnly = true,
            Secure = true
        });
        return Ok(new { Message = "Logout sucessful" });
    }
}