using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MinimalBankAPI.Bussines.Base.Rules;
using MinimalBankAPI.Bussines.Features.Roles.Dtos;
using MinimalBankAPI.Bussines.Features.Roles.Rules;
using MinimalBankAPI.DataAccess.Repositories.Abstract;
using MinimalBankAPI.DataAccess.UnitOfWorks;
using MinimalBankAPI.Domain.Entites.Auth;

namespace MinimalBankAPI.Bussines.Features.Roles.Services
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleOperationClaimRepository _roleOperationClaimRepository;
        private readonly RoleRules _roleRules;
        public RoleService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IRoleRepository roleRepository, RoleRules roleRules, IRoleOperationClaimRepository roleOperationClaimRepository) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _roleRepository = roleRepository;
            _roleRules = roleRules;
            _roleOperationClaimRepository = roleOperationClaimRepository;
        }

        public async Task CreateAsync(CreateRoleDto dto)
        {
            Role? role = await _roleRepository.GetAsync(x=>x.Name == dto.Name);
            await _roleRules.EnsureRoleIsNotExists(role);
            var data = _mapper.Map<Role>(dto);
            var currentUserId = !string.IsNullOrEmpty(_userId) ? Guid.Parse(_userId) : (Guid?)null;
            data.CreatedBy = currentUserId;
            await _roleRepository.AddAsync(data);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<RoleDto>> GetAllAsync()
        {
            int size = 2147483647;
            var index = 1;
            var roles = await _roleRepository.GetAllByPagingAsync(pageSize: size,currentPage:index,include:i => i.Include(x=>x.RoleOperationClaims));
            var result = _mapper.Map<List<RoleDto>>(roles);
            return result;
        }

        // SETROLEOPERATİON CLAİMS
        public async Task SetPermissionsAsync(SetRoleOperationClaimsDto dto)
        {
            var role = await _roleRepository.GetAsync(x => x.Id == dto.RoleId,
                include: i => i.Include(x => x.RoleOperationClaims),
                enableTracking: true);
            await _roleRules.EnsureIsRoleExists(role);
            var currentUserId = !string.IsNullOrEmpty(_userId) ? Guid.Parse(_userId) : (Guid?)null;
            role.RoleOperationClaims.Clear();
            foreach(var operationClaimId in dto.OperationClaimIds)
            {
                var roleOperationClaim = new RoleOperationClaim
                {
                    CreatedBy = currentUserId,
                    RoleId = role.Id,
                    OperationClaimId = operationClaimId
                };
                await _roleOperationClaimRepository.AddAsync(roleOperationClaim);
                role.RoleOperationClaims.Add(roleOperationClaim);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateRoleDto dto)
        {
            Role? role = await _roleRepository.GetAsync(x => x.Id == dto.Id,enableTracking:true);
            await _roleRules.EnsureIsRoleExists(role);
            _mapper.Map(dto, role);
            var currentUserId = !string.IsNullOrEmpty(_userId) ? Guid.Parse(_userId) : (Guid?)null;
            role.UpdatedDate = DateTime.UtcNow;
            role.UpdatedBy = currentUserId;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
