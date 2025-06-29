using MemberTestAPI.Dtos;
using MemberTestAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MemberTestAPI.Repositories.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task CreateRole(CreateRoleDto roleDto)
        {
            if (roleDto == null)
                throw new ArgumentException("Role name cannot be empty");

            if (await _roleManager.RoleExistsAsync(roleDto.Name))
            {
                throw new Exception($"Role '{roleDto.Name}' already exists");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleDto.Name));

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create role: {string.Join(", ", result.Errors.Select(x => x.Description))}");
            }
        }


        public async Task<List<string>> GetAllRoles()
        {
            return await _roleManager.Roles.Select(x => x.Name).ToListAsync();
        }

        public async Task DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                throw new ArgumentException($"Role with Name {roleName} does not exist.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to delete role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

    }
}
