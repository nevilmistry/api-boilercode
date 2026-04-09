using GenricRepository.Application.Contracts.Users;

namespace GenricRepository.Application.Handlers.Users;

public interface IUserCommandHandler
{
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task<UserResponse?> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<bool> DeleteAsync(Guid id);
}
