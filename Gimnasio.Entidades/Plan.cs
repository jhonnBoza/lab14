using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gimnasio.Entidades;

[Table("Plan")]
public class Plan
{
    public int PlanId { get; set; }

    [MaxLength(80)]
    public string Nombre { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecioMensual { get; set; }

    public int DuracionMeses { get; set; }

    [MaxLength(300)]
    public string? Descripcion { get; set; }

    public ICollection<Miembro> Miembros { get; set; } = new List<Miembro>();
}
