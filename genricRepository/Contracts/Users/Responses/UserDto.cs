namespace genricRepository.Contracts.Users.Responses
{
    public class UserDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Guid RoleId { get; set; }
    }
}
