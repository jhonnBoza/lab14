using System.ComponentModel.DataAnnotations;

namespace Gimnasio.Entidades.Dtos;

public class PlanDto
{
    public int PlanId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre admite hasta 80 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Range(0, 5000, ErrorMessage = "El precio mensual debe estar entre 0 y 5000.")]
    public decimal PrecioMensual { get; set; }

    [Range(1, 36, ErrorMessage = "La duración debe estar entre 1 y 36 meses.")]
    public int DuracionMeses { get; set; }

    [StringLength(300, ErrorMessage = "La descripción admite hasta 300 caracteres.")]
    public string? Descripcion { get; set; }
}
