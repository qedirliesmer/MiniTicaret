using MiniTicaret.Application.DTOs.AuthenticationDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTicaret.Application.Abstracts.Services;

public interface IAuthenticationService
{
    Task<AuthenticationTokenResponseDto> RegisterAsync(AuthenticationRegisterDto registerDto);
    Task<AuthenticationTokenResponseDto> LoginAsync(AuthenticationLoginDto loginDto);
    Task<AppUser> GetMeAsync(string userId);

}
