namespace genricRepository.Model
{
    public class Role : BaseRepository
    {
        public string? Name { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
