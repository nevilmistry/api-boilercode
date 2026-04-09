using GenricRepository.Application.Abstractions.Persistence;
using GenricRepository.Application.Contracts.Common;
using GenricRepository.Application.Contracts.Users;
using GenricRepository.Domain.Entities;

namespace GenricRepository.Application.Handlers.Users;

public sealed class UserQueryHandler : IUserQueryHandler
{
    private readonly IUserRepository _userRepository;

    public UserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedResult<UserResponse>> GetAllAsync(UsersListQuery query)
    {
        var normalized = NormalizeQuery(query);
        var (items, totalCount) = await _userRepository.GetPagedAsync(normalized);
        var mapped = items.Select(Map).ToList();
        var totalPages = (int)Math.Ceiling(totalCount / (double)normalized.PageSize);

        return new PagedResult<UserResponse>(
            mapped,
            normalized.Page,
            normalized.PageSize,
            totalCount,
            totalPages);
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user is null ? null : Map(user);
    }

    private static UsersListQuery NormalizeQuery(UsersListQuery query)
    {
        return new UsersListQuery
        {
            Page = query.Page < 1 ? 1 : query.Page,
            PageSize = query.PageSize switch
            {
                < 1 => 20,
                > 100 => 100,
                _ => query.PageSize
            },
            Search = query.Search?.Trim(),
            RoleId = query.RoleId,
            SortBy = string.IsNullOrWhiteSpace(query.SortBy) ? "name" : query.SortBy.Trim(),
            SortOrder = string.Equals(query.SortOrder, "desc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc"
        };
    }

    private static UserResponse Map(User user) => new(
        user.Id,
        user.Name,
        user.Email,
        user.RoleId,
        user.Role?.Name ?? string.Empty);
}
