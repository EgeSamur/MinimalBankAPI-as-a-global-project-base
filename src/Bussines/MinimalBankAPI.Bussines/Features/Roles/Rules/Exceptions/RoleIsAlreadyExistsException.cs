using MinimalBankAPI.Bussines.Base.Rules;

namespace MinimalBankAPI.Bussines.Features.Users.Rules.Exceptions
{
    public class RoleIsAlreadyExistsException : BaseException
    {
        public RoleIsAlreadyExistsException() : base("Role is already exists.") { }
    }
}
