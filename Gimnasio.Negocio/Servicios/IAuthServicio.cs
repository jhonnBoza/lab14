using Gimnasio.Entidades.Dtos;

namespace Gimnasio.Negocio.Servicios;

public interface IAuthServicio
{
    Task<PerfilDto> RegistrarAsync(RegistroDto dto);

    Task<TokenDto?> LoginAsync(LoginDto dto);
}
