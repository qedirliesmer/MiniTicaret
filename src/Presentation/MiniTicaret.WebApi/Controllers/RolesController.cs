using Microsoft.AspNetCore.Mvc;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Application.DTOs.RoleDTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiniTicaret.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        // POST: /api/roles
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto dto)
        {
            await _roleService.CreateAsync(dto);
            return StatusCode(201); // Created
        }

        // DELETE: /api/roles/{roleName}
        [HttpDelete("{roleName}")]
        public async Task<IActionResult> Delete(string roleName)
        {
            await _roleService.DeleteAsync(roleName);
            return NoContent(); // 204
        }

    }
}
