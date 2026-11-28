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

    [StringLength(150, ErrorMessage = "La ubicación no puede superar los 150 caracteres")]
    public string Ubicacion { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "La distancia no puede ser negativa")]
    public double DistanciaRecorrido { get; set; }

    [StringLength(250, ErrorMessage = "El espacio disponible no puede superar los 250 caracteres")]
    public string EspacioDisponible { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La descripción técnica no puede superar los 1000 caracteres")]
    public string DescripcionTecnica { get; set; } = string.Empty;

    public bool Viable { get; set; } = true;
}