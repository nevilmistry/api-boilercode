using AutoMapper;
using genricRepository.Contracts.Roles.Requests;
using genricRepository.Contracts.Roles.Responses;
using genricRepository.Model;
using genricRepository.Repository;

namespace genricRepository.Application
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleResponse>> GetAllAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleResponse>>(roles);
        }

        public async Task<RoleResponse?> GetByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            return role is null ? null : _mapper.Map<RoleResponse>(role);
        }

        public async Task<RoleResponse> CreateAsync(CreateRoleRequest request)
        {
            var role = _mapper.Map<Role>(request);
            role.Id = Guid.NewGuid();

            await _roleRepository.AddAsync(role);
            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<RoleResponse?> UpdateAsync(Guid id, UpdateRoleRequest request)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role is null)
            {
                return null;
            }

            _mapper.Map(request, role);
            await _roleRepository.UpdateAsync(role);
            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<DeleteRoleResponse?> DeleteAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role is null)
            {
                return null;
            }

            await _roleRepository.DeleteAsync(role);
            return new DeleteRoleResponse
            {
                Id = id,
                Message = "Role deleted successfully."
            };
        }
    }
}
