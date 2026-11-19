using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gimnasio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthServicio auth) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("registro")]
    public async Task<ActionResult<PerfilDto>> Registro(RegistroDto dto)
    {
        var perfil = await auth.RegistrarAsync(dto);
        return StatusCode(StatusCodes.Status201Created, perfil);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        var token = await auth.LoginAsync(dto);
        if (token is null)
        {
            return Unauthorized(new { mensaje = "Correo o clave incorrectos." });
        }

        return Ok(token);
    }

    [Authorize]
    [HttpGet("perfil")]
    public ActionResult<PerfilDto> Perfil()
    {
        var correo = User.FindFirst("email")?.Value;
        var rol = User.FindFirst("role")?.Value;
        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(rol))
        {
            return Unauthorized();
        }

        return Ok(new PerfilDto
        {
            Correo = correo,
            Rol = rol
        });
    }
}
