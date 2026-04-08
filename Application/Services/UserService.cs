using GenricRepository.Application.Abstractions.Persistence;
using GenricRepository.Application.Contracts.Users;
using GenricRepository.Domain.Entities;

namespace GenricRepository.Application.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<IReadOnlyCollection<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(Map).ToList();
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user is null ? null : Map(user);
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        await EnsureRoleExists(request.RoleId);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            RoleId = request.RoleId
        };

        await _userRepository.AddAsync(user);
        var createdUser = await _userRepository.GetByIdAsync(user.Id) ?? user;
        return Map(createdUser);
    }

    public async Task<UserResponse?> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return null;
        }

        await EnsureRoleExists(request.RoleId);

        user.Name = request.Name.Trim();
        user.Email = request.Email.Trim();
        user.RoleId = request.RoleId;

        await _userRepository.UpdateAsync(user);
        var updatedUser = await _userRepository.GetByIdAsync(user.Id) ?? user;
        return Map(updatedUser);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return false;
        }

        await _userRepository.DeleteAsync(user);
        return true;
    }

    private async Task EnsureRoleExists(Guid roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role is null)
        {
            throw new InvalidOperationException("Invalid role id.");
        }
    }

    private static UserResponse Map(User user) => new(
        user.Id,
        user.Name,
        user.Email,
        user.RoleId,
        user.Role?.Name ?? string.Empty);
}
