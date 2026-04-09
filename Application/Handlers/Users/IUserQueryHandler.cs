using GenricRepository.Application.Contracts.Common;
using GenricRepository.Application.Contracts.Users;

namespace GenricRepository.Application.Handlers.Users;

public interface IUserQueryHandler
{
    Task<PagedResult<UserResponse>> GetAllAsync(UsersListQuery query);
    Task<UserResponse?> GetByIdAsync(Guid id);
}
