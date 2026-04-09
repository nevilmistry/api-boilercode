using GenricRepository.Application.Abstractions.Persistence;
using GenricRepository.Application.Contracts.Common;
using GenricRepository.Application.Contracts.Roles;
using GenricRepository.Domain.Entities;

namespace GenricRepository.Application.Handlers.Roles;

public sealed class RoleQueryHandler : IRoleQueryHandler
{
    private readonly IRoleRepository _roleRepository;

    public RoleQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<PagedResult<RoleResponse>> GetAllAsync(RolesListQuery query)
    {
        var normalized = NormalizeQuery(query);
        var (items, totalCount) = await _roleRepository.GetPagedAsync(normalized);
        var mapped = items.Select(Map).ToList();
        var totalPages = (int)Math.Ceiling(totalCount / (double)normalized.PageSize);

        return new PagedResult<RoleResponse>(
            mapped,
            normalized.Page,
            normalized.PageSize,
            totalCount,
            totalPages);
    }

    public async Task<RoleResponse?> GetByIdAsync(Guid id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        return role is null ? null : Map(role);
    }

    private static RolesListQuery NormalizeQuery(RolesListQuery query)
    {
        return new RolesListQuery
        {
            Page = query.Page < 1 ? 1 : query.Page,
            PageSize = query.PageSize switch
            {
                < 1 => 20,
                > 100 => 100,
                _ => query.PageSize
            },
            Search = query.Search?.Trim(),
            SortBy = string.IsNullOrWhiteSpace(query.SortBy) ? "name" : query.SortBy.Trim(),
            SortOrder = string.Equals(query.SortOrder, "desc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc"
        };
    }

    private static RoleResponse Map(Role role) => new(role.Id, role.Name);
}
