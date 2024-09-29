using MinimalBankAPI.Bussines.Base.Rules;
using MinimalBankAPI.Bussines.Features.Users.Rules.Exceptions;
using MinimalBankAPI.Domain.Entites.Auth;
using MinimalBankAPI.Security.Hashing;

namespace MinimalBankAPI.Bussines.Features.Users.Rules
{
    public class UserRules : BaseRules
    {
        public Task EnsureUserIsNotExists(User? user)
        {
            if(user is not null)
            {
                throw new UserIsAlreadyExistsException();
            }
            return Task.CompletedTask;
        }

        public Task EnsureIsUserExists(User? user)
        {
            if (user is null)
            {
                throw new UserDoesNotExistException();
            }
            return Task.CompletedTask;
        }

    
    }
}
