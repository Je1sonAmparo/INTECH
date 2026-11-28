using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTECH.Models;

public class LevantamientosDetalle
{
    [Key]
    public int DetalleId { get; set; }

    public int LevantamientoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un servicio")]
    public int ServicioId { get; set; }
}