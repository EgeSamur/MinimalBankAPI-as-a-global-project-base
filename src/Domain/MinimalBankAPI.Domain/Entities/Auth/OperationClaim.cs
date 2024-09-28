using MinimalBankAPI.Domain.Common;

namespace MinimalBankAPI.Domain.Entites.Auth;

public class OperationClaim : BaseEntity
{
    public string Name { get; set; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; }

    public ICollection<RoleOperationClaim> RoleOperationClaims { get; set; }
}
