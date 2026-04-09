using GenricRepository.Application.Contracts.Common;
using GenricRepository.Application.Contracts.Roles;

namespace GenricRepository.Application;

public interface IRoleService
{
    Task<PagedResult<RoleResponse>> GetAllAsync(RolesListQuery query);
    Task<RoleResponse?> GetByIdAsync(Guid id);
    Task<RoleResponse> CreateAsync(CreateRoleRequest request);
    Task<RoleResponse?> UpdateAsync(Guid id, UpdateRoleRequest request);
    Task<bool> DeleteAsync(Guid id);
}
