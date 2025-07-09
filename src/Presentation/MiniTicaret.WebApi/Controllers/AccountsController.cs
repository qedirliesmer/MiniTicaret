using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.AccountDTOs;
using MiniTicaret.Application.DTOs.AssignDTOs;
using MiniTicaret.Application.Shared.Permissions;
using MiniTicaret.Domain.Entities;
using System.Security.Claims;

namespace MiniTicaret.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("create-role")]
    [Authorize(Policy = "Account.AddRole")]

    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        var success = await _accountService.CreateRoleAsync(roleName);
        if (!success)
            return BadRequest("Role already exists or failed to create.");

        return Ok("Role created successfully.");
    }

    [HttpPost("assign-role")]
    [Authorize(Policy = "Account.AddRole")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
    {
        var success = await _accountService.AssignRoleToUserAsync(model.UserId, model.RoleName);
        if (!success)
            return BadRequest("User not found, role does not exist, or failed to assign.");

        return Ok("Role assigned successfully.");
    }

   
}

