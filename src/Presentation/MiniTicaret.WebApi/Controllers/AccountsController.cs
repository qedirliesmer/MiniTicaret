using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.AccountDTOs;
using MiniTicaret.Application.Shared.Permissions;
using System.Security.Claims;

namespace MiniTicaret.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _service;
    private readonly IHttpContextAccessor _httpContext;

    public AccountsController(IAccountService service, IHttpContextAccessor httpContext)
    {
        _service = service;
        _httpContext = httpContext;
    }

    [HttpPut("profile")]
    [Authorize(Policy = Permissions.Account.UpdateProfile)]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _service.UpdateProfileAsync(userId, dto);
        return NoContent();
    }

    [HttpPut("change-password")]
    [Authorize(Policy = Permissions.Account.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _service.ChangePasswordAsync(userId, dto);
        return NoContent();
    }
}
