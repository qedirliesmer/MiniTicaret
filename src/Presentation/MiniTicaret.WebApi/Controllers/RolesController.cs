using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.RoleDTOs;
using MiniTicaret.Application.Shared.Permissions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniTicaret.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        [Authorize(Policy = Permissions.Role.GetAllPermission)]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        // POST: /api/roles
        [HttpPost]
        [Authorize(Policy = Permissions.Role.Create)]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto dto)
        {
            await _roleService.CreateAsync(dto);
            return StatusCode(201); // Created
        }
        [HttpPut("{roleName}")]
        [Authorize(Policy = Permissions.Role.Update)]
        
        public async Task<IActionResult> Update(string roleName, [FromBody] RoleUpdateDto dto)
        {
            if (roleName != dto.OldName)
                return BadRequest("Role name in URL and payload do not match.");

            await _roleService.UpdateAsync(dto);
            return NoContent();
        }

    }
}
