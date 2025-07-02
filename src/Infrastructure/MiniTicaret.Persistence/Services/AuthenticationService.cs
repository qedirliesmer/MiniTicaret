using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.AuthenticationDTOs;
using MiniTicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace MiniTicaret.Persistence.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthenticationService(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config,
        IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
        _contextAccessor = contextAccessor;
    }

    public async Task<AuthenticationTokenResponseDto> RegisterAsync(AuthenticationRegisterDto dto)
    {
        var user = new AppUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        if (!await _roleManager.RoleExistsAsync(dto.Role))
            await _roleManager.CreateAsync(new IdentityRole(dto.Role));

        await _userManager.AddToRoleAsync(user, dto.Role);

        return await GenerateToken(user);
    }

    public async Task<AuthenticationTokenResponseDto> LoginAsync(AuthenticationLoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new Exception("Email və ya şifrə yanlışdır");

        return await GenerateToken(user);
    }

    public async Task<AppUser> GetMeAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new Exception("İstifadəçi tapılmadı");
        return user;
    }

    private async Task<AuthenticationTokenResponseDto> GenerateToken(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };
        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(1);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthenticationTokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = "", // refresh token əlavə etməmisənsə, boş saxla
            ExpireDate = expires
        };
    }
   
}
