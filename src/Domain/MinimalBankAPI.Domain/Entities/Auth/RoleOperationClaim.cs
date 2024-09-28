using MinimalBankAPI.Domain.Common;

namespace MinimalBankAPI.Domain.Entites.Auth;

public class RoleOperationClaim : BaseEntity
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    public Guid OperationClaimId { get; set; }
    public OperationClaim OperationClaim { get; set; }
}

