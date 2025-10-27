using Lab10_AlberthMayta.Application.DTOs;
using Lab10_AlberthMayta.Application.usecases;
using Microsoft.AspNetCore.Mvc;
// <-- Importa tu Caso de Uso

namespace Lab10_AlberthMayta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthUseCase _authUseCase;

        // 1. Inyecta el Caso de Uso
        public AuthController(AuthUseCase authUseCase)
        {
            _authUseCase = authUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                // 2. Llama al Caso de Uso
                var userDto = await _authUseCase.RegisterAsync(request);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Manejo simple de errores
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                // 3. Llama al Caso de Uso
                var authResponse = await _authUseCase.LoginAsync(request);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message); // Manejo simple de errores
            }
        }
    }
}