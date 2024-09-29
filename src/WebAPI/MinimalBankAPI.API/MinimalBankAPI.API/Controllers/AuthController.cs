using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalBankAPI.Bussines.Features.Auth.Dtos;
using MinimalBankAPI.Bussines.Features.Auth.Services;

namespace MinimalBankAPI.API.Controllers
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
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _service.RegisterAsync(dto);
            return Ok(result);
        }
    }
}
