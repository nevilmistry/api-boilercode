using genricRepository.Model;
using genricRepository.Repository.BaseRepository;

namespace genricRepository.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
    }
}
