using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.AuthenticationDTOs;
using MiniTicaret.Application.Exceptions;
using MiniTicaret.Application.Shared.Responses;
using MiniTicaret.Application.Shared.Settings;
using MiniTicaret.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace MiniTicaret.Persistence.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JWTSettings _jwtSettings;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    public AuthenticationService(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JWTSettings> jwtOptions,
        ILogger<AuthenticationService> logger,
        IMapper mapper,
        IEmailService _emailService,
        IConfiguration _configuration,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtOptions.Value;
        _logger = logger;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    private string GenerateRefreshToken()
    {
        byte[] bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return WebEncoders.Base64UrlEncode(bytes);
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
        if (user == null)//STARE AT THEN
        {
            _logger.LogWarning("Invalid login attempt for email: {Email}", dto.Email);
            throw new ValidationException(new[] { "Email or password is incorrect." });
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, true);
        if (!signInResult.Succeeded)//STARE AT THEN
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

    //private async Task<AuthenticationTokenResponseDto> GenerateToken(AppUser user)
    //{
    //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


    //    var claims = new List<Claim>
    //    {
    //        new(ClaimTypes.NameIdentifier, user.Id),
    //        new(ClaimTypes.Name, user.UserName)
    //    };
    //    var roles = await _userManager.GetRolesAsync(user);

    //    foreach (var roleName in roles)
    //    {
    //        claims.Add(new Claim(ClaimTypes.Role, roleName));

    //        // var role = await _roleManager.FindByNameAsync(roleName);
    //        //if (role != null)
    //        //{
    //        //    var roleClaims = await _roleManager.GetClaimsAsync(role);//buda 0 gelirdi

    //        //    var permissionClaims = roleClaims// buda 0 gelirdi//BOSHDU AXI DB ONA GORE   NECE YENI TAM ANLAMADIM DEYER DAXIL ETMELIYDIM?
    //        //        .Where(c => c.Type == "Permission")
    //        //        .Distinct()
    //        //        .ToList();

    //        //     foreach (var permissionClaim in permissionClaims)
    //        //    {
    //        //        if (!claims.Any(c => c.Type == "Permission" && c.Value == permissionClaim.Value))
    //        //        {
    //        //            claims.Add(new Claim("Permission", permissionClaim.Value));
    //        //        }
    //        //    }
    //        //}
    //    }
    //    //foreach (var roleName in roles)
    //    //{
    //    //    if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == roleName))
    //    //    {
    //    //        claims.Add(new Claim(ClaimTypes.Role, roleName));
    //    //    }

    //    //    var role = await _roleManager.FindByNameAsync(roleName);
    //    //    if (role != null)
    //    //    {
    //    //        var roleClaims = await _roleManager.GetClaimsAsync(role);

    //    //        foreach (var permissionClaim in roleClaims
    //    //            .Where(c => c.Type == "Permission")
    //    //            .Distinct())
    //    //        {
    //    //            if (!claims.Any(c => c.Type == "Permission" && c.Value == permissionClaim.Value))
    //    //            {
    //    //                claims.Add(new Claim("Permission", permissionClaim.Value));
    //    //            }
    //    //        }
    //    //    }
    //    //}

    //    var token = new JwtSecurityToken(
    //        issuer: _jwtSettings.Issuer,
    //        audience: _jwtSettings.Audience,
    //        notBefore: DateTime.UtcNow,
    //        claims: claims,
    //        expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.ExpiryMinutes)),
    //        signingCredentials: creds);

    //    var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
    //    var refreshToken = GenerateRefreshToken();

    //    user.RefreshToken = refreshToken;
    //    user.ExpiryDate = DateTime.UtcNow.AddMinutes(30);//
    //    await _userManager.UpdateAsync(user);
    //    var tokenResponse = new TokenResponse
    //    {
    //        Token = accessToken,
    //        RefreshToken = refreshToken,
    //        ExpireDate = user.ExpiryDate
    //    };
    //    var dto = _mapper.Map<AuthenticationTokenResponseDto>(tokenResponse);
    //    return dto;
    //}

    private async Task<AuthenticationTokenResponseDto> GenerateToken(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var roleName in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var permissionClaim in roleClaims
                    .Where(c => c.Type == "Permission")
                    .Distinct())
                {
                    if (!claims.Any(c => c.Type == "Permission" && c.Value == permissionClaim.Value))
                    {
                        claims.Add(new Claim("Permission", permissionClaim.Value));
                    }
                }
            }
        }

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            notBefore: DateTime.UtcNow,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.ExpiryMinutes)),
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.ExpiryDate = DateTime.UtcNow.AddMinutes(30);
        await _userManager.UpdateAsync(user);

        var tokenResponse = new TokenResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpireDate = user.ExpiryDate
        };

        var dto = _mapper.Map<AuthenticationTokenResponseDto>(tokenResponse);
        return dto;
    }
    public async Task SendEmailConfirmationAsync(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = Uri.EscapeDataString(token);

        var confirmUrl = $"{_configuration["AppSettings:FrontendUrl"]}/confirm-email?userId={user.Id}&token={encodedToken}";

        var message = $"<p>Salam, zəhmət olmasa email ünvanınızı təsdiqləyin: <a href='{confirmUrl}'>Təsdiq et</a></p>";

        await _emailService.SendEmailAsync(
            new List<string> { user.Email },
            "Email təsdiqi",
            message
        );
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<bool> CanUserLoginAsync(AppUser user)
    {
        return await _userManager.IsEmailConfirmedAsync(user);
    }

    public async Task RegisterUserAsync(AppUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

        await SendEmailConfirmationAsync(user);
    }
  

}



