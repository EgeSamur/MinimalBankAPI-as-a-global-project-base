using MinimalBankAPI.DataAccess.Repositories.Abstract.Base;
using MinimalBankAPI.Domain.Entites.Auth;

namespace MinimalBankAPI.DataAccess.Repositories.Abstract
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
    }
}
