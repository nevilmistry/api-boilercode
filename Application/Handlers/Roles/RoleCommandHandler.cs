using GenricRepository.Application.Abstractions.Persistence;
using GenricRepository.Application.Contracts.Roles;
using GenricRepository.Domain.Entities;

namespace GenricRepository.Application.Handlers.Roles;

public sealed class RoleCommandHandler : IRoleCommandHandler
{
    private readonly IRoleRepository _roleRepository;

    public RoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleResponse> CreateAsync(CreateRoleRequest request)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim()
        };

        await _roleRepository.AddAsync(role);
        return Map(role);
    }

    public async Task<RoleResponse?> UpdateAsync(Guid id, UpdateRoleRequest request)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role is null)
        {
            return null;
        }

        role.Name = request.Name.Trim();
        await _roleRepository.UpdateAsync(role);
        return Map(role);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role is null)
        {
            return false;
        }

        await _roleRepository.DeleteAsync(role);
        return true;
    }

    private static RoleResponse Map(Role role) => new(role.Id, role.Name);
}
