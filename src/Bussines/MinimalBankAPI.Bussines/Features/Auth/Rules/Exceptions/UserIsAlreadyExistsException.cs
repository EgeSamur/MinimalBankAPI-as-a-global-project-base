using MinimalBankAPI.Bussines.Base.Rules;

namespace MinimalBankAPI.Bussines.Features.Auth.Rules.Exceptions
{
    public class UserIsAlreadyExistsException : BaseException
    {
        public UserIsAlreadyExistsException() : base("User is already exists.") { }
    }
}
