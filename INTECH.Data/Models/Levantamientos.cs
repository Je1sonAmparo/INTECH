using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTECH.Models;

public class Levantamientos
{
    [Key]
    public int LevantamientoId { get; set; }

    public bool IsDeleted { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una solicitud")]
    public int SolicitudId { get; set; }

    public string? TecnicoId { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public DateTime FechaProgramada { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public TimeOnly HoraInicio { get; set; } = new TimeOnly(8, 0);

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public TimeOnly HoraFin { get; set; } = new TimeOnly(9, 30);

    public string EstabilidadTension { get; set; } = string.Empty;

    public string RecorridoTuberias { get; set; } = string.Empty;

    public string RecorridoCableado { get; set; } = string.Empty;

    public string Observaciones { get; set; } = string.Empty;

    public bool EsBorrador { get; set; }

    [ForeignKey("SolicitudId")]
    [InverseProperty("Levantamientos")]
    public virtual Solicitudes Solicitud { get; set; } = null!;

    [InverseProperty("Levantamiento")]
    public virtual ICollection<FotosLevantamientos> FotosLevantamientos { get; set; } = new List<FotosLevantamientos>();
}
