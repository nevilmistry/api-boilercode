using genricRepository.Data;
using genricRepository.Model;
using genricRepository.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace genricRepository.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
