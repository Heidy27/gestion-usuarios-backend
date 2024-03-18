using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gestionDeUsuario.Application.Interfaces;
using gestionDeUsuario.Application.DTOs;
using System.Threading.Tasks;
using System.Security.Claims;

namespace gestionDeUsuario.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;  

        public UsersController(IUserService userService, ILogService logService)  // Constructor modificado para incluir ILogService
        {
            _userService = userService;
            _logService = logService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(registerDto);

            if (result.Succeeded)
            {
                // Registro de log para la creación de usuario
                await _logService.SaveLogAsync(User.Identity.Name, "Creación de usuario", $"Usuario {registerDto.Username} creado con éxito.");
                return Ok(new { message = "User created successfully." });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            if (userDto != null)
            {
                // Registro de log para la consulta de usuario
                await _logService.SaveLogAsync(User.Identity.Name, "Consulta de usuario", $"Consulta del usuario con ID {id} realizada con éxito.");
                return Ok(userDto);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var userDtos = await _userService.GetAllUsersAsync();
            // Registro de log para la consulta de todos los usuarios
            await _logService.SaveLogAsync(User.Identity.Name, "Consulta de todos los usuarios", "Consulta de todos los usuarios realizada con éxito.");
            return Ok(userDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (id != updateUserDto.Id)
            {
                return BadRequest("Mismatched user ID");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdateUserAsync(updateUserDto);

            if (result.Succeeded)
            {
                // Registro de log para la actualización de usuario
                await _logService.SaveLogAsync(User.Identity.Name, "Actualización de usuario", $"Usuario con ID {id} actualizado con éxito.");
                return Ok(new { message = "User updated successfully." });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "Token inválido." });
            }

            // Asume que tienes un método en tu servicio para obtener la información del usuario por ID
            var user = await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound(new { message = "Usuario no encontrado." });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (result.Succeeded)
            {
                // Registro de log para la eliminación de usuario
                await _logService.SaveLogAsync(User.Identity.Name, "Eliminación de usuario", $"Usuario con ID {id} eliminado con éxito.");
                return Ok(new { message = "User deleted successfully." });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
