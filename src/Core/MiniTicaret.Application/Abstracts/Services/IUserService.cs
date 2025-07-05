using MiniTicaret.Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Services;

public interface IUserService
{
    Task<List<UserGetDto>> GetAllUsersAsync();
    Task<UserDetailDto> GetUserByIdAsync(string id);
    Task<List<UserGetDto>> GetAllAsync();
    Task<UserGetDto?> GetByIdAsync(string id);
}
