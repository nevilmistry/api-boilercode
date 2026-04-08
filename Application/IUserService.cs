using GenricRepository.Application.Contracts.Users;

namespace GenricRepository.Application;

public interface IUserService
{
    Task<IReadOnlyCollection<UserResponse>> GetAllAsync();
    Task<UserResponse?> GetByIdAsync(Guid id);
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task<UserResponse?> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<bool> DeleteAsync(Guid id);
}
