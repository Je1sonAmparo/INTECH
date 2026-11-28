using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTECH.Models;

public class SolicitudesDetalle
{
    [Key]
    public int DetalleId { get; set; }

    public int SolicitudId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un servicio")]
    public int ServicioId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
    public int Cantidad { get; set; } = 1;

    [ForeignKey("SolicitudId")]
    [InverseProperty("SolicitudesDetalle")]
    public virtual Solicitudes Solicitud { get; set; } = null!;

    [ForeignKey("ServicioId")]
    [InverseProperty("SolicitudesDetalle")]
    public virtual Servicios Servicio { get; set; } = null!;
}
