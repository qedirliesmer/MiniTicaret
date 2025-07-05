using AutoMapper;
using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Persistence.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<UserGetDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersWithRolesAsync();
        var userDtos = new List<UserGetDto>();

        foreach (var user in users)
        {
            var roles = await _userRepository.GetUserRolesAsync(user);
            userDtos.Add(new UserGetDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles
            });
        }

        return userDtos;
    }

    public async Task<UserDetailDto> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdWithRelationsAsync(id);
        if (user == null)
            throw new Exception("User not found");

        var roles = await _userRepository.GetUserRolesAsync(user);

        return new UserDetailDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Roles = roles,
            CreatedAt = user.CreatedAt,
            ProductCount = user.Products?.Count ?? 0,
            OrderCount = user.Orders?.Count ?? 0,
            ReviewCount = user.Reviews?.Count ?? 0,
            FavoriteCount = user.Favorites?.Count ?? 0
        };
    }
    public async Task<List<UserGetDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<List<UserGetDto>>(users);
    }

    public async Task<UserGetDto?> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;
        return _mapper.Map<UserGetDto>(user);
    }
  
}
