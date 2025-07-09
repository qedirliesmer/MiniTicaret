using Microsoft.AspNetCore.Identity;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.AccountDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Services;

public class AccountService : IAccountService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<bool> CreateRoleAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
            return false;

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        return result.Succeeded;
    }

    public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        if (!await _roleManager.RoleExistsAsync(roleName))
            return false;

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }
}
