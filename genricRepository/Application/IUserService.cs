using genricRepository.Contracts.Users.Requests;
using genricRepository.Contracts.Users.Responses;

namespace genricRepository.Application
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<UserDto> CreateUserAsync(CreateUserRequest request);
        Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserRequest request);
        Task<DeleteUserResponse?> DeleteUserAsync(Guid id);
    }
}
