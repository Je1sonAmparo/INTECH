using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTECH.Models;

public class Solicitudes
{
    [Key]
    public int SolicitudId { get; set; }

    public bool IsDeleted { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    [StringLength(250, ErrorMessage = "La dirección no puede superar los 250 caracteres")]
    public string Direccion { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Las observaciones no pueden superar los 1000 caracteres")]
    public string ObservacionesCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public DateTime FechaSolicitud { get; set; } = DateTime.Today;

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado")]
    public int EstadoSolicitudId { get; set; } = 1;

    [ForeignKey("ClienteId")]
    [InverseProperty("Solicitudes")]
    public virtual Clientes Cliente { get; set; } = null!;

    [ForeignKey("EstadoSolicitudId")]
    [InverseProperty("Solicitudes")]
    public virtual EstadosSolicitudes EstadoSolicitud { get; set; } = null!;

    [InverseProperty("Solicitud")]
    public virtual ICollection<SolicitudesDetalle> SolicitudesDetalle { get; set; } = new List<SolicitudesDetalle>();

    [InverseProperty("Solicitud")]
    public virtual ICollection<Levantamientos> Levantamientos { get; set; } = new List<Levantamientos>();

    [InverseProperty("Solicitud")]
    public virtual ICollection<Agendas> Agendas { get; set; } = new List<Agendas>();

    [InverseProperty("Solicitud")]
    public virtual ICollection<OrdenesTrabajo> OrdenesTrabajo { get; set; } = new List<OrdenesTrabajo>();

    [InverseProperty("Solicitud")]
    public virtual ICollection<Historiales> Historiales { get; set; } = new List<Historiales>();
}
