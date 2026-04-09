using GenricRepository.Application.Contracts.Common;
using GenricRepository.Application.Contracts.Users;

namespace GenricRepository.Application;

public interface IUserService
{
    Task<PagedResult<UserResponse>> GetAllAsync(UsersListQuery query);
    Task<UserResponse?> GetByIdAsync(Guid id);
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task<UserResponse?> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<bool> DeleteAsync(Guid id);
}
