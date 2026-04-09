using GenricRepository.Application.Contracts.Common;
using GenricRepository.Application.Contracts.Roles;

namespace GenricRepository.Application.Handlers.Roles;

public interface IRoleQueryHandler
{
    Task<PagedResult<RoleResponse>> GetAllAsync(RolesListQuery query);
    Task<RoleResponse?> GetByIdAsync(Guid id);
}
