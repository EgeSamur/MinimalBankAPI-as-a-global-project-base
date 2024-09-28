using MinimalBankAPI.Domain.Common;

namespace MinimalBankAPI.Domain.Entites.Auth;

public class Role : BaseEntity
{
    public string Name { get; set; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<RoleOperationClaim> RoleOperationClaims { get; set; }
}

