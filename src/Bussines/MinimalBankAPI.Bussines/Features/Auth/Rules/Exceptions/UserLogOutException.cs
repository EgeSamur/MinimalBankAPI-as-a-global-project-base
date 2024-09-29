using MinimalBankAPI.Bussines.Base.Rules;

namespace MinimalBankAPI.Bussines.Features.Auth.Rules.Exceptions
{
    public class UserLogOutException : BaseException
    {
        public UserLogOutException() : base("User logged out.") { }
    }
}
