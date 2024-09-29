using Microsoft.AspNetCore.Mvc;
using MinimalBankAPI.Bussines.Features.Auth.Dtos;
using MinimalBankAPI.Bussines.Features.Auth.Services;
using MinimalBankAPI.Bussines.Features.Roles.Dtos;

namespace MinimalBankAPI.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService authService)
        {
            _service = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _service.LoginAsync(dto);
            return Ok(result);
        }
        //[HttpPost]
        //public async Task<IActionResult> Revoke([FromBody] RevokeDto dto)
        //{
        //    await _service.RevokeAsync(dto);
        //    return Ok();
        //}
        [HttpPost]
        public async Task<IActionResult> RevokeAll()
        {
            await _service.RevokeAllAsync();
            return Ok();
        }

    }
}
