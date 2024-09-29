namespace MinimalBankAPI.Bussines.Features.Roles.Dtos
{
    public class SetRoleOperationClaimsDto
    {
        public Guid RoleId { get; set; }
        public List<Guid> OperationClaimIds { get; set; }
    }
}
