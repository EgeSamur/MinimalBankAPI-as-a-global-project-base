using MinimalBankAPI.Bussines.Base.Rules;

namespace MinimalBankAPI.Bussines.Features.Users.Rules.Exceptions
{
    public class UserDoesNotExistException : BaseException
    {
        public UserDoesNotExistException() : base("User does not exist.") { }
    }
}
