namespace GenricRepository.Domain.Entities;

public sealed class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}
