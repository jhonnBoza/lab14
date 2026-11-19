using Gimnasio.Entidades;
using Gimnasio.Entidades.Excepciones;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos.Repositorios;

public class MiembroRepositorio(GimnasioDbContext db) : IMiembroRepositorio
{
    public async Task<List<Miembro>> ListarAsync(string? apellidos, int? planId, string? email)
    {
        var consulta = db.Miembros.AsNoTracking().Include(m => m.Plan).AsQueryable();

        if (!string.IsNullOrWhiteSpace(apellidos))
        {
            consulta = consulta.Where(m => m.Apellidos.Contains(apellidos));
        }

        if (planId is not null)
        {
            consulta = consulta.Where(m => m.PlanId == planId);
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            consulta = consulta.Where(m => m.Email.ToLower() == email.ToLower());
        }

        return await consulta.OrderBy(m => m.MiembroId).ToListAsync();
    }

    public Task<Miembro?> ObtenerAsync(int id) =>
        db.Miembros.AsNoTracking()
            .Include(m => m.Plan)
            .FirstOrDefaultAsync(m => m.MiembroId == id);

    public Task<bool> ExisteEmailAsync(string email, int? exceptoId = null) =>
        db.Miembros.AnyAsync(m =>
            m.Email.ToLower() == email.ToLower() &&
            (exceptoId == null || m.MiembroId != exceptoId));

    public async Task AgregarAsync(Miembro miembro) =>
        await db.Miembros.AddAsync(miembro);

    public async Task<bool> ActualizarAsync(Miembro datos)
    {
        var actual = await db.Miembros.FirstOrDefaultAsync(m => m.MiembroId == datos.MiembroId);
        if (actual is null)
        {
            return false;
        }

        actual.Nombres = datos.Nombres;
        actual.Apellidos = datos.Apellidos;
        actual.Email = datos.Email;
        actual.Telefono = datos.Telefono;
        actual.FechaInscripcion = datos.FechaInscripcion;
        actual.PlanId = datos.PlanId;
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var miembro = await db.Miembros.FirstOrDefaultAsync(m => m.MiembroId == id);
        if (miembro is null)
        {
            return false;
        }

        db.Miembros.Remove(miembro);
        return true;
    }

    public async Task GuardarAsync()
    {
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new ReglaNegocioException("Ya existe un miembro con ese correo.", 409);
        }
    }
}
