using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTicaret.Application.DTOs.AuthenticationDTOs;
using System.Security.Claims;
using MiniTicaret.Application.Abstracts.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniTicaret.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(IAuthenticationService authService, IHttpContextAccessor httpContextAccessor)
    {
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthenticationRegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticationLoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _authService.GetMeAsync(userId!);
        return Ok(new { user.FullName, user.Email, user.Id });
    }
}

