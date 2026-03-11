using genricRepository.Contracts.Roles.Requests;
using genricRepository.Contracts.Roles.Responses;

namespace genricRepository.Application
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponse>> GetAllAsync();
        Task<RoleResponse?> GetByIdAsync(Guid id);
        Task<RoleResponse> CreateAsync(CreateRoleRequest request);
        Task<RoleResponse?> UpdateAsync(Guid id, UpdateRoleRequest request);
        Task<DeleteRoleResponse?> DeleteAsync(Guid id);
    }
}
