using Microsoft.AspNetCore.Mvc;
using MinimalBankAPI.Bussines.Features.OperationClaims.Dtos;
using MinimalBankAPI.Bussines.Features.OperationClaims.Services;

namespace MinimalBankAPI.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OperationClaimsController : ControllerBase
    {
        private readonly IOperationClaimService _service;

        public OperationClaimsController(IOperationClaimService operationClaimService)
        {
            _service = operationClaimService;
        }
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOperationClaimDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOperationClaimDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }
    }
}
