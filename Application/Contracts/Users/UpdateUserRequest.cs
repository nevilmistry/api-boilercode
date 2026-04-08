namespace GenricRepository.Application.Contracts.Users;

public sealed record UpdateUserRequest(string Name, string Email, Guid RoleId);
