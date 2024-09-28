using MinimalBankAPI.Domain.Common;

namespace MinimalBankAPI.Domain.Entites.Auth;

public class User : BaseEntity
    {
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool Status { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }

