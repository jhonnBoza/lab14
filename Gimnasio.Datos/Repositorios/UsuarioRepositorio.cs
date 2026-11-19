using Gimnasio.Entidades;
using Gimnasio.Entidades.Excepciones;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos.Repositorios;

public class UsuarioRepositorio(GimnasioDbContext db) : IUsuarioRepositorio
{
    public Task<Usuario?> BuscarPorCorreoAsync(string correo) =>
        db.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Correo == correo);

    public async Task AgregarAsync(Usuario usuario) =>
        await db.Usuarios.AddAsync(usuario);

    public async Task GuardarAsync()
    {
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new ReglaNegocioException("Ya existe un usuario con ese correo.", 409);
        }
    }
}
