using AutoMapper;
using MinimalBankAPI.Bussines.Features.OperationClaims.Dtos;
using MinimalBankAPI.Domain.Entites.Auth;

namespace MinimalBankAPI.Bussines.Features.OperationClaims.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<OperationClaim, OperationClaimsDto>().ReverseMap();
            CreateMap<OperationClaim, CreateOperationClaimDto>().ReverseMap();
            CreateMap<OperationClaim, UpdateOperationClaimDto>().ReverseMap();
        }
    }
}
