using MemberTestAPI.Dtos;
using MemberTestAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberTestAPI.Controllers
{
    [Authorize(Roles ="admin")]
    [Route("api/administration")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public AdministrationController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            if (ModelState.IsValid)
            {
                await _roleService.CreateRole(createRoleDto);
                return Ok($"Role {createRoleDto.Name} created successfully");
            }
            return BadRequest(ModelState);
        }

        [HttpGet("get-roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _roleService.GetAllRoles());
        }

        [HttpDelete("{roleName}/delete-role")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            await _roleService.DeleteRole(roleName);
            return Ok($"Role with Name '{roleName}' deleted.");
        }

    }
}
