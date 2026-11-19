using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
using Gimnasio.Entidades.Excepciones;
using Microsoft.IdentityModel.Tokens;

namespace Gimnasio.Negocio.Servicios;

public class AuthServicio(IUsuarioRepositorio usuarios, JwtOpciones jwt) : IAuthServicio
{
    public async Task<PerfilDto> RegistrarAsync(RegistroDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();

        var existente = await usuarios.BuscarPorCorreoAsync(correo);
        if (existente is not null)
        {
            throw new ReglaNegocioException("Ya existe un usuario con ese correo.", 409);
        }

        var usuario = new Usuario
        {
            Correo = correo,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
            Rol = RolesNegocio.PorDefecto
        };

        await usuarios.AgregarAsync(usuario);
        await usuarios.GuardarAsync();

        return new PerfilDto
        {
            Correo = usuario.Correo,
            Rol = usuario.Rol
        };
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();
        var usuario = await usuarios.BuscarPorCorreoAsync(correo);

        if (usuario is null || !ClaveValida(dto.Clave, usuario.ClaveHash))
        {
            return null;
        }

        var expira = DateTime.UtcNow.AddMinutes(jwt.ExpiracionMinutos);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.UsuarioId.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Correo),
            new("role", usuario.Rol)
        };

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims: claims,
            expires: expira,
            signingCredentials: credenciales);

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracion = expira
        };
    }

    private static bool ClaveValida(string clave, string hash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(clave, hash);
        }
        catch (Exception)
        {
            return false;
        }
    }
}
