using Gimnasio.Entidades;

namespace Gimnasio.Datos.Repositorios;

public interface IMiembroRepositorio
{
    Task<List<Miembro>> ListarAsync(string? apellidos, int? planId, string? email);

    Task<Miembro?> ObtenerAsync(int id);

    Task<bool> ExisteEmailAsync(string email, int? exceptoId = null);

    Task AgregarAsync(Miembro miembro);

    Task<bool> ActualizarAsync(Miembro datos);

    Task<bool> EliminarAsync(int id);

    Task GuardarAsync();
}
