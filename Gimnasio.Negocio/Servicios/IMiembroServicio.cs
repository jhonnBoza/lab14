using Gimnasio.Entidades.Dtos;

namespace Gimnasio.Negocio.Servicios;

public interface IMiembroServicio
{
    Task<IReadOnlyList<MiembroDto>> ListarAsync(string? apellidos, int? planId, string? email);

    Task<MiembroDto> ObtenerAsync(int id);

    Task<MiembroDto> CrearAsync(MiembroDto dto);

    Task ActualizarAsync(int id, MiembroDto dto);

    Task EliminarAsync(int id);
}
