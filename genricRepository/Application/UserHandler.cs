using AutoMapper;
using genricRepository.Contracts.Users.Requests;
using genricRepository.Contracts.Users.Responses;
using genricRepository.Model;
using genricRepository.Repository;

namespace genricRepository.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role is null)
            {
                throw new InvalidOperationException("Invalid role id.");
            }

            var user = _mapper.Map<User>(request);
            user.Id = Guid.NewGuid();

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role is null)
            {
                throw new InvalidOperationException("Invalid role id.");
            }

            _mapper.Map(request, user);

            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<DeleteUserResponse?> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            await _userRepository.DeleteAsync(user);
            return new DeleteUserResponse
            {
                Id = id,
                Message = "User deleted successfully."
            };
        }
    }
}
