using MinimalBankAPI.Bussines.Features.Auth.Dtos;

namespace MinimalBankAPI.Bussines.Features.Auth.Services
{
    public interface IAuthService
    {
        Task<LoggedDto> LoginAsync(LoginDto dto);
       
        //Task RevokeAsync(RevokeDto dto);
        Task RevokeAllAsync();
    }
}
