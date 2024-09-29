using MinimalBankAPI.Bussines.Base.Rules;
using MinimalBankAPI.Bussines.Features.Auth.Rules.Exceptions;
using MinimalBankAPI.Domain.Entites.Auth;
using MinimalBankAPI.Security.Hashing;
using UserDoesNotExistException = MinimalBankAPI.Bussines.Features.Auth.Rules.Exceptions.UserDoesNotExistException;
using UserIsAlreadyExistsException = MinimalBankAPI.Bussines.Features.Auth.Rules.Exceptions.UserIsAlreadyExistsException;

namespace MinimalBankAPI.Bussines.Features.Auth.Rules
{
    public class AuthRules : BaseRules
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

        public async Task EnsurePasswordMatches(User user, string password)
        {
            if (!HashingHelper.VerifyPasswordHash(password, user!.PasswordHash, user.PasswordSalt))
                throw new Exception("Login information not verified");
            await Task.CompletedTask;
        }

        public Task EnsureUserNotLogOut(DateTime? refreshTokenExpireTime)
        {
            if (refreshTokenExpireTime <= DateTime.Now)
            {
                throw new UserLogOutException();
            }
            return Task.CompletedTask;
        }
    }
}
