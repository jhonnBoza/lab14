using System.ComponentModel.DataAnnotations;

namespace Gimnasio.Entidades.Dtos;

public class MiembroDto
{
    public int MiembroId { get; set; }

    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(80, ErrorMessage = "Los nombres admiten hasta 80 caracteres.")]
    public string Nombres { get; set; } = null!;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(80, ErrorMessage = "Los apellidos admiten hasta 80 caracteres.")]
    public string Apellidos { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [StringLength(120, ErrorMessage = "El correo admite hasta 120 caracteres.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    public string Email { get; set; } = null!;

    [StringLength(20, ErrorMessage = "El teléfono admite hasta 20 caracteres.")]
    public string? Telefono { get; set; }

    public DateOnly FechaInscripcion { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar el id de un plan.")]
    public int PlanId { get; set; }

    public string? NombrePlan { get; set; }
}
