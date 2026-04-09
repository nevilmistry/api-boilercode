using GenricRepository.Application.Contracts.Roles;

namespace GenricRepository.Application.Handlers.Roles;

public interface IRoleCommandHandler
{
    Task<RoleResponse> CreateAsync(CreateRoleRequest request);
    Task<RoleResponse?> UpdateAsync(Guid id, UpdateRoleRequest request);
    Task<bool> DeleteAsync(Guid id);
}
