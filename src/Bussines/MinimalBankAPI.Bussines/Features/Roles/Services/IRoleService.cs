
using MinimalBankAPI.Bussines.Features.Roles.Dtos;

namespace MinimalBankAPI.Bussines.Features.Roles.Services
{
    public interface IRoleService
    {
        Task<IList<RoleDto>> GetAllAsync();
        Task CreateAsync(CreateRoleDto dto);
        Task UpdateAsync(UpdateRoleDto dto);
        Task SetPermissionsAsync(SetRoleOperationClaimsDto dto);
    }
}
