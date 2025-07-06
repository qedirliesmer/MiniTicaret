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
using MiniTicaret.Application.Shared.Permissions;
using Microsoft.Extensions.Options;
using MiniTicaret.Application.Shared.Settings;
using Microsoft.Extensions.Logging;
using MiniTicaret.Application.Exceptions;
namespace MiniTicaret.Persistence.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JWTSettings _jwtSettings;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JWTSettings> jwtOptions,
        ILogger<AuthenticationService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtOptions.Value;
        _logger = logger;
    }

    public async Task<AuthenticationTokenResponseDto> RegisterAsync(AuthenticationRegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Registration attempt with existing email: {Email}", dto.Email);
            throw new ValidationException(new[] { $"User '{dto.Email}' already exists." });
        }

        var user = new AppUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            _logger.LogWarning("User creation failed for email {Email}: {Errors}", dto.Email, result.Errors.Select(e => e.Description));
            throw new ValidationException(result.Errors.Select(e => e.Description));
        }

        if (!await _roleManager.RoleExistsAsync(dto.Role))
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(dto.Role));
            if (!roleResult.Succeeded)
            {
                _logger.LogError("Role creation failed: {Errors}", roleResult.Errors.Select(e => e.Description));
                throw new Exception("Failed to create user role.");
            }
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, dto.Role);
        if (!addRoleResult.Succeeded)
        {
            _logger.LogError("Adding role to user failed: {Errors}", addRoleResult.Errors.Select(e => e.Description));
            throw new Exception("Failed to assign role to user.");
        }

        _logger.LogInformation("User registered successfully: {Email}", dto.Email);

        return await GenerateToken(user);
    }

    public async Task<AuthenticationTokenResponseDto> LoginAsync(AuthenticationLoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            _logger.LogWarning("Invalid login attempt for email: {Email}", dto.Email);
            throw new ValidationException(new[] { "Email or password is incorrect." });
        }

        _logger.LogInformation("User logged in successfully: {Email}", dto.Email);

        return await GenerateToken(user);
    }

    public async Task<AppUser> GetMeAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User not found with Id: {UserId}", userId);
            throw new Exception("User not found");
        }
        return user;
    }

    private async Task<AuthenticationTokenResponseDto> GenerateToken(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? throw new Exception("UserName is null")),
            new Claim(ClaimTypes.Email, user.Email ?? throw new Exception("Email is null"))
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));

            var permissions = Permissions.GetPermissionsByRole(role);
            foreach (var permission in permissions.Distinct())
            {
                claims.Add(new Claim("Permission", permission));
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        _logger.LogInformation("JWT Token generated for user {UserId}", user.Id);

        return new AuthenticationTokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = "", // refresh token varsa əlavə edə bilərsən
            ExpireDate = expires
        };
    }
}