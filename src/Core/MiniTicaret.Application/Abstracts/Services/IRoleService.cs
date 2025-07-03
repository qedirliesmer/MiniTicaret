using MiniTicaret.Application.DTOs.RoleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Services;

public interface IRoleService
{
    Task<List<RoleGetDto>> GetAllAsync();
    Task CreateAsync(RoleCreateDto dto);
    Task DeleteAsync(string roleName);
}
