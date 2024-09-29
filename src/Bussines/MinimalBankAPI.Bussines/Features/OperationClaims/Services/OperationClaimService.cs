using AutoMapper;
using Microsoft.AspNetCore.Http;
using MinimalBankAPI.Bussines.Base.Rules;
using MinimalBankAPI.Bussines.Features.OperationClaims.Dtos;
using MinimalBankAPI.DataAccess.Repositories.Abstract;
using MinimalBankAPI.DataAccess.UnitOfWorks;
using MinimalBankAPI.Domain.Entites.Auth;

namespace MinimalBankAPI.Bussines.Features.OperationClaims.Services
{
    public class OperationClaimService : BaseService, IOperationClaimService
    {
        private readonly IOperationClaimRepository _operationClaimRepository;

        public OperationClaimService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IOperationClaimRepository operationClaimRepository) : base(mapper, unitOfWork, httpContextAccessor)
        {
            _operationClaimRepository = operationClaimRepository;
        }

        public async Task CreateAsync(CreateOperationClaimDto dto)
        {
            var data = _mapper.Map<OperationClaim>(dto);
            var currentUserId = !string.IsNullOrEmpty(_userId) ? Guid.Parse(_userId) : (Guid?)null;
            data.Id = Guid.NewGuid();
            data.CreatedBy = currentUserId;
            await _operationClaimRepository.AddAsync(data);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<OperationClaimsDto>> GetAllAsync()
        {
            int MaxValue = 2147483647;
            var pageIndexx = 1;
            var operationClaims = await _operationClaimRepository.GetAllByPagingAsync(currentPage: pageIndexx, pageSize: MaxValue,
                predicate: p => p.IsDeleted == false);
            var result = _mapper.Map<IList<OperationClaimsDto>>(operationClaims);
            return result;
        }

        public async Task UpdateAsync(UpdateOperationClaimDto dto)
        {
            var operationClaim = await _operationClaimRepository.GetAsync(x => x.Id == dto.Id, enableTracking: true);
            _mapper.Map(dto,operationClaim);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
