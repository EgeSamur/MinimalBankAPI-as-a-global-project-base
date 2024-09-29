using Microsoft.AspNetCore.Mvc;
using MinimalBankAPI.Bussines.Features.Users.Dtos;
using MinimalBankAPI.Bussines.Features.Users.Services;

namespace MinimalBankAPI.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var result = await _service.GetListAsync();
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _service.ResetPasswordAsync(dto);
            return Ok();
        }


        [HttpPost("set-roles")]
        public async Task<IActionResult> SetUserRoles([FromBody] SetUserRolesDto dto)
        {
            await _service.SetRolesAsync(dto);
            return Ok();
        }

    }
}
