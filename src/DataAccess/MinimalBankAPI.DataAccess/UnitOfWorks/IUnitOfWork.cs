namespace MinimalBankAPI.DataAccess.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task SaveChangesAsync();
    }
}
