using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTECH.Models;

public class FotosLevantamientos
{
    [Key]
    public int FotoLevantamientoId { get; set; }

    public int DetalleId { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public string NombreArchivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public string TipoContenido { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public byte[] Contenido { get; set; } = Array.Empty<byte>();

    [ForeignKey("DetalleId")]
    [InverseProperty("FotosLevantamientos")]
    public virtual Levantamientos Levantamiento { get; set; } = null!;
}
