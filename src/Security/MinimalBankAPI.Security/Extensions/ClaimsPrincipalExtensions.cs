using System.Security.Claims;

namespace MinimalBankAPI.Security.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result;
        }

        public static List<string>? ClaimRoles(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal?.Claims(ClaimTypes.Role);

        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal?.Claims(ClaimTypes.NameIdentifier)?.FirstOrDefault();
            if (id is not null)
            {
                Guid.TryParse(id, out Guid userId);
                return userId;
            }
            return Guid.Empty;
        }
    }
}
