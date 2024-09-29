using MinimalBankAPI.Bussines.Base.Rules;

namespace MinimalBankAPI.Bussines.Features.Users.Rules.Exceptions
{
    public class UserIsAlreadyExistsException : BaseException
    {
        public UserIsAlreadyExistsException() : base("User is already exists.") { }
    }
}
