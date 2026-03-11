using genricRepository.Data;
using genricRepository.Model;
using genricRepository.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace genricRepository.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(role => role.Name == name);
        }
    }
}
