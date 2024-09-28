using MinimalBankAPI.Domain.Common;

namespace MinimalBankAPI.Domain.Entites.Auth;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Token { get; set; }
    public DateTime RefreshTokenExpirationTime { get; set; }
}

