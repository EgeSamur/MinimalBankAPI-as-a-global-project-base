using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MinimalBankAPI.Domain.Entites.Auth;
using MinimalBankAPI.Security.Encryption;
using MinimalBankAPI.Security.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MinimalBankAPI.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private readonly TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            const string configurationSection = "Jwt";
            _tokenOptions =
                Configuration.GetSection(configurationSection).Get<TokenOptions>()
                ?? throw new NullReferenceException($"\"{configurationSection}\" section cannot found in configuration.");
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);

        }
        public AccessToken CreateToken(User user)
        {
            _accessTokenExpiration = DateTime.Now.AddHours(_tokenOptions.AccessTokenExpiration);
            var _refreshTokenExpriration = DateTime.Now.AddDays(_tokenOptions.RefreshTokenTTL);
            var refreshToken = GenerateRefreshToken();
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            string? token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = _refreshTokenExpriration,
            };
        }


        public JwtSecurityToken CreateJwtSecurityToken(
            TokenOptions tokenOptions,
            User user,
            SigningCredentials signingCredentials
        )
        {
            JwtSecurityToken jwt =
                new(
                    tokenOptions.Issuer,
                    tokenOptions.Audience,
                    expires: _accessTokenExpiration,
                    notBefore: DateTime.Now,
                    claims: SetClaims(user),
                    signingCredentials: signingCredentials
                );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user)
        {
            // Kullanıcıya ait id'yi ekler
            List<Claim> claims = new();
            claims.AddNameIdentifier(user.Id.ToString());
            // kullanıcın e mailinde yazarız
            claims.AddEmail(user.EmailAddress);

            // Kullanıcının rollerine ait role-claim'leri alır
            var roleClaims = user.UserRoles
                .SelectMany(ur => ur.Role.RoleOperationClaims
                    .Select(rc => rc.OperationClaim.Name))
                .ToList();

            // Kullanıcının rollerini alır
            var roles = user.UserRoles
                .Select(ur => ur.Role.Name)
                .ToArray();

            
            // Kullanıcının rollerini claim olarak ekler
            claims.AddRoles(roles);

            // Kullanıcının role-claim'lerini (yetkilerini) claim olarak ekler
            claims.AddPermissions(roleClaims);


            return claims;
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey)),
                ValidateLifetime = false,
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
    !jwtSecurityToken.Header.Alg.Contains("hmac-sha256", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token Bulunamadı.");
            }

            return principal;
        }
    }

}
