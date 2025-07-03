using Microsoft.AspNetCore.Identity;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.RoleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager=roleManager;
    }
    public async Task<List<RoleGetDto>> GetAllAsync()
    {
        var roles = _roleManager.Roles.ToList();
        return roles.Select(r => new RoleGetDto { Name = r.Name }).ToList();
    }
    public async Task CreateAsync(RoleCreateDto dto)
    {
        var exists = await _roleManager.RoleExistsAsync(dto.Name);
        if (exists)
            throw new Exception("Bu rol artıq mövcuddur");

        var result = await _roleManager.CreateAsync(new IdentityRole(dto.Name));
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

    }

    public async Task DeleteAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            throw new Exception("Rol tapılmadı");

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    
}
