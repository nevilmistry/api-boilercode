using GenricRepository.Application.Abstractions.Persistence;
using GenricRepository.Domain.Entities;
using GenricRepository.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GenricRepository.Infrastructure.Repositories;

public sealed class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Role>> GetAllAsync()
    {
        return await _context.Roles.AsNoTracking().ToListAsync();
    }

    public async Task<Role?> GetByIdAsync(Guid id)
    {
        return await _context.Roles.FirstOrDefaultAsync(role => role.Id == id);
    }

    public async Task<Role> AddAsync(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task UpdateAsync(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Role role)
    {
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
    }
}
