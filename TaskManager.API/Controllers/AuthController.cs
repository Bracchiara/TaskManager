using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Business.DTOs.Auth;
using TaskManager.Business.Interfaces;

namespace TaskManager.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    // Register a new user
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        try
        {
            var response = await _authService.RegisterAsync(registerDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Login and get JWT token
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            var response = await _authService.LoginAsync(loginDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
