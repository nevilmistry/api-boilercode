using AutoMapper;
using genricRepository.Contracts.Roles.Requests;
using genricRepository.Contracts.Roles.Responses;
using genricRepository.Model;

namespace genricRepository.Mapping
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();
            CreateMap<Role, RoleResponse>();
        }
    }
}
