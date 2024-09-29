using MinimalBankAPI.Bussines.Features.OperationClaims.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalBankAPI.Bussines.Features.OperationClaims.Services
{
    public interface IOperationClaimService
    {
        Task<IList<OperationClaimsDto>> GetAllAsync();
        Task CreateAsync(CreateOperationClaimDto dto);
        Task UpdateAsync(UpdateOperationClaimDto dto);
    }
}
