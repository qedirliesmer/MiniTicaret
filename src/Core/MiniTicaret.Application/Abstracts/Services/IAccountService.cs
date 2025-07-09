using MiniTicaret.Application.DTOs.AccountDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Services;

public interface IAccountService
{
    Task<bool> CreateRoleAsync(string roleName);
    Task<bool> AssignRoleToUserAsync(string userId, string roleName);
}
