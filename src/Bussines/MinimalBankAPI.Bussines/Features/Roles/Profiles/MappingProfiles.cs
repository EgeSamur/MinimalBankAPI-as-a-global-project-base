using AutoMapper;
using MinimalBankAPI.Bussines.Features.Roles.Dtos;
using MinimalBankAPI.Domain.Entites.Auth;

namespace MinimalBankAPI.Bussines.Features.Roles.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Role, RoleDto>().ForMember(dest => dest.OperationClaims,
                opt => opt.MapFrom(src => src.RoleOperationClaims.Select(rp => rp.OperationClaim)));
            CreateMap<Role, CreateRoleDto>().ReverseMap();
            CreateMap<Role, UpdateRoleDto>().ReverseMap();
        }
    }
}
