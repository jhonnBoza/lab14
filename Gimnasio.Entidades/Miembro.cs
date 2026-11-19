using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gimnasio.Entidades;

[Table("Miembro")]
public class Miembro
{
    public int MiembroId { get; set; }

    [MaxLength(80)]
    public string Nombres { get; set; } = null!;

    [MaxLength(80)]
    public string Apellidos { get; set; } = null!;

    [MaxLength(120)]
    public string Email { get; set; } = null!;

    [MaxLength(20)]
    public string? Telefono { get; set; }

    public DateOnly FechaInscripcion { get; set; }

    public int PlanId { get; set; }

    [ForeignKey(nameof(PlanId))]
    public Plan? Plan { get; set; }
}
