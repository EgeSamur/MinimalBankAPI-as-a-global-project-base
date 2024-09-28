using Microsoft.IdentityModel.Tokens;

namespace MinimalBankAPI.Security.Encryption
{
    public static class SigningCredentialsHelper
    {
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey) =>
            new(securityKey, SecurityAlgorithms.HmacSha256Signature);
    }
}
