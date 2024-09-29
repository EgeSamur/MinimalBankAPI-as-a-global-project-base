using MinimalBankAPI.Bussines.Features.OperationClaims.Dtos;
using MinimalBankAPI.Bussines.Features.Roles.Dtos;

namespace MinimalBankAPI.Bussines.Features.Users.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public bool Status { get; set; }
        public List<OperationClaimsDto> OperationClaims { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
