using genricRepository.Model;
using genricRepository.Repository.BaseRepository;

namespace genricRepository.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email); // only user-specific
    }
}
