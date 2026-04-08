namespace GenricRepository.Application.Contracts.Users;

public sealed record UserResponse(Guid Id, string Name, string Email, Guid RoleId, string RoleName);
