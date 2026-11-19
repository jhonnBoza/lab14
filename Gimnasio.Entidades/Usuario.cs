using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gimnasio.Entidades;

[Table("Usuario")]
public class Usuario
{
    public int UsuarioId { get; set; }

    [MaxLength(120)]
    public string Correo { get; set; } = null!;

    [MaxLength(200)]
    public string ClaveHash { get; set; } = null!;

    [MaxLength(40)]
    public string Rol { get; set; } = null!;
}
