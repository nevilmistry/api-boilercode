using AutoMapper;
using genricRepository.Contracts.Users.Requests;
using genricRepository.Contracts.Users.Responses;
using genricRepository.Model;

namespace genricRepository.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UserDto>();
        }
    }
}
