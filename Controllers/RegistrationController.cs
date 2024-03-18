using Microsoft.AspNetCore.Mvc;
using gestionDeUsuario.Application.DTOs;
using gestionDeUsuario.Application.Interfaces;
using System.Threading.Tasks;

namespace gestionDeUsuario.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _registrationService.RegisterUserAsync(registerDto);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Registro exitoso." });
            }
            else
            {
                     return BadRequest(result.Errors);
            }
        }
    }
}
