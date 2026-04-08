using GenricRepository.Domain.Entities;

namespace GenricRepository.Application.Abstractions.Persistence;

public interface IRoleRepository
{
    Task<IReadOnlyCollection<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(Guid id);
    Task<Role> AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(Role role);
}
