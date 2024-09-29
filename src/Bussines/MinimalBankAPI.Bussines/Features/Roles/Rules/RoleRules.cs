using MinimalBankAPI.Bussines.Base.Rules;
using MinimalBankAPI.Bussines.Features.Users.Rules.Exceptions;
using MinimalBankAPI.Domain.Entites.Auth;

namespace MinimalBankAPI.Bussines.Features.Roles.Rules
{
    public class RoleRules : BaseRules
    {
        public Task EnsureRoleIsNotExists(Role? user)
        {
            if(user is not null)
            {
                throw new RoleIsAlreadyExistsException();
            }
            return Task.CompletedTask;
        }

        public Task EnsureIsRoleExists(Role? user)
        {
            if (user is null)
            {
                throw new RoleDoesNotExistException();
            }
            return Task.CompletedTask;
        }
    }
}
