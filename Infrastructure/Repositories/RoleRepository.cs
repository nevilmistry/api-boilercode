using GenricRepository.Application.Abstractions.Persistence;
using GenricRepository.Application.Contracts.Roles;
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

    public async Task<(IReadOnlyCollection<Role> Items, int TotalCount)> GetPagedAsync(RolesListQuery query)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize switch
        {
            < 1 => 20,
            > 100 => 100,
            _ => query.PageSize
        };

        var rolesQuery = _context.Roles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            rolesQuery = rolesQuery.Where(role => role.Name.Contains(search));
        }

        rolesQuery = ApplySorting(rolesQuery, query.SortBy, query.SortOrder);

        var totalCount = await rolesQuery.CountAsync();
        var items = await rolesQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
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

    private static IQueryable<Role> ApplySorting(IQueryable<Role> rolesQuery, string? sortBy, string? sortOrder)
    {
        var isDesc = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);
        var sortKey = sortBy?.Trim().ToLowerInvariant();

        return (sortKey, isDesc) switch
        {
            ("id", true) => rolesQuery.OrderByDescending(role => role.Id),
            ("id", false) => rolesQuery.OrderBy(role => role.Id),
            ("name", true) => rolesQuery.OrderByDescending(role => role.Name),
            (_, true) => rolesQuery.OrderByDescending(role => role.Name),
            _ => rolesQuery.OrderBy(role => role.Name)
        };
    }
}
