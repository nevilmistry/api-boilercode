using GenricRepository.Domain.Entities;

namespace GenricRepository.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<IReadOnlyCollection<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}
