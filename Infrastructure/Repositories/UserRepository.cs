using GenricRepository.Application.Abstractions.Persistence;
using GenricRepository.Application.Contracts.Users;
using GenricRepository.Domain.Entities;
using GenricRepository.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GenricRepository.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyCollection<User> Items, int TotalCount)> GetPagedAsync(UsersListQuery query)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize switch
        {
            < 1 => 20,
            > 100 => 100,
            _ => query.PageSize
        };

        var usersQuery = _context.Users
            .AsNoTracking()
            .Include(user => user.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            usersQuery = usersQuery.Where(user =>
                user.Name.Contains(search) ||
                user.Email.Contains(search));
        }

        if (query.RoleId.HasValue)
        {
            usersQuery = usersQuery.Where(user => user.RoleId == query.RoleId.Value);
        }

        usersQuery = ApplySorting(usersQuery, query.SortBy, query.SortOrder);

        var totalCount = await usersQuery.CountAsync();
        var items = await usersQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(user => user.Role)
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    private static IQueryable<User> ApplySorting(IQueryable<User> usersQuery, string? sortBy, string? sortOrder)
    {
        var isDesc = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);
        var sortKey = sortBy?.Trim().ToLowerInvariant();

        return (sortKey, isDesc) switch
        {
            ("id", true) => usersQuery.OrderByDescending(user => user.Id),
            ("id", false) => usersQuery.OrderBy(user => user.Id),
            ("email", true) => usersQuery.OrderByDescending(user => user.Email),
            ("email", false) => usersQuery.OrderBy(user => user.Email),
            ("role", true) => usersQuery.OrderByDescending(user => user.Role != null ? user.Role.Name : string.Empty),
            ("role", false) => usersQuery.OrderBy(user => user.Role != null ? user.Role.Name : string.Empty),
            ("name", true) => usersQuery.OrderByDescending(user => user.Name),
            (_, true) => usersQuery.OrderByDescending(user => user.Name),
            _ => usersQuery.OrderBy(user => user.Name)
        };
    }
}
