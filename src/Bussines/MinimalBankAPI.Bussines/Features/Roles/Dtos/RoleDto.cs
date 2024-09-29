using MinimalBankAPI.Bussines.Features.OperationClaims.Dtos;

namespace MinimalBankAPI.Bussines.Features.Roles.Dtos
{
    public class RoleDto
    {

        public string Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public List<OperationClaimsDto> OperationClaims { get; set; }

    }
}
