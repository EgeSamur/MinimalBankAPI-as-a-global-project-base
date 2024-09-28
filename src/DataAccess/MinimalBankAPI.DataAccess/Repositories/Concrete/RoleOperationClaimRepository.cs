using Microsoft.EntityFrameworkCore;
using MinimalBankAPI.DataAccess.Context;
using MinimalBankAPI.DataAccess.Repositories.Abstract;
using MinimalBankAPI.DataAccess.Repositories.Concrete.Base;
using MinimalBankAPI.Domain.Entites.Auth;

namespace MinimalBankAPI.DataAccess.Repositories.Concrete
{
    public class RoleOperationClaimRepository : BaseRepository<RoleOperationClaim>, IRoleOperationClaimRepository
    {
        public RoleOperationClaimRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
