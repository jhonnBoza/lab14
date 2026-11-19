using System.ComponentModel.DataAnnotations;

namespace Gimnasio.Entidades.Dtos;

public class RegistroDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(120)]
    public string Correo { get; set; } = null!;

    [Required(ErrorMessage = "La clave es obligatoria.")]
    [StringLength(80, MinimumLength = 6, ErrorMessage = "La clave debe tener entre 6 y 80 caracteres.")]
    public string Clave { get; set; } = null!;
}
