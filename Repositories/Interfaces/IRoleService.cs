using MemberTestAPI.Dtos;

namespace MemberTestAPI.Repositories.Interfaces
{
    public interface IRoleService
    {
        Task CreateRole(CreateRoleDto roleDto);
        Task<List<string>> GetAllRoles();
        Task DeleteRole(string name);
    }
}
