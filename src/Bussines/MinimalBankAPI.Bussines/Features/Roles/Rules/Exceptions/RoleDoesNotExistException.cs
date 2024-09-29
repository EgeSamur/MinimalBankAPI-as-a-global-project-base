using MinimalBankAPI.Bussines.Base.Rules;

namespace MinimalBankAPI.Bussines.Features.Users.Rules.Exceptions
{
    public class RoleDoesNotExistException : BaseException
    {
        public RoleDoesNotExistException() : base("Role does not exist.") { }
    }
}
