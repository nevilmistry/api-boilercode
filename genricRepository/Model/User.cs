namespace genricRepository.Model
{
    public class User : BaseRepository
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
