namespace GenricRepository.Application.Contracts.Users;

public sealed record CreateUserRequest(string Name, string Email, Guid RoleId);
