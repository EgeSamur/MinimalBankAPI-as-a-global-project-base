using MinimalBankAPI.Domain.Entites.Auth;
using System.Security.Claims;

namespace MinimalBankAPI.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }


}
