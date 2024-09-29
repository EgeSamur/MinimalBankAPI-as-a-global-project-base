
using MinimalBankAPI.Bussines.Features.Users.Dtos;

namespace MinimalBankAPI.Bussines.Features.Users.Services
{
    public interface IUserService
    {
        Task CreateAsync(CreateUserDto dto);
        Task UpdateAsync(UpdateUserDto dto);
        Task DeleteAsync(Guid id);
        Task<IList<UserDto>> GetListAsync();
        Task ResetPasswordAsync(ResetPasswordDto dto);
        // role permision yani claim atama olacak.
        //Task SetPermissionsAsync(SetUserPermissionsDto dto);
        Task SetRolesAsync(SetUserRolesDto dto);
    }
}
