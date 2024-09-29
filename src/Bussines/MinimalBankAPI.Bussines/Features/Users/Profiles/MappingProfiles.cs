using AutoMapper;
using MinimalBankAPI.Bussines.Features.Users.Dtos;
using MinimalBankAPI.Domain.Entites.Auth;

namespace MinimalBankAPI.Bussines.Features.Users.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>()
               .ForMember(dest => dest.Roles,
               opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)));
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<User, ResetPasswordDto>().ReverseMap();

        }
    }
}
