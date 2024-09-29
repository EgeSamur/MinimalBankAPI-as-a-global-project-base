using Microsoft.AspNetCore.Mvc;
using MinimalBankAPI.Bussines.Features.Roles.Dtos;
using MinimalBankAPI.Bussines.Features.Roles.Services;

namespace MinimalBankAPI.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _service;

        public RolesController(IRoleService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoleDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok();
        }
        [HttpPost("set-permissions")]
        public async Task<IActionResult> SetPermissions([FromBody] SetRoleOperationClaimsDto dto)
        {
            await _service.SetPermissionsAsync(dto);
            return Ok();
        }
    }
}
