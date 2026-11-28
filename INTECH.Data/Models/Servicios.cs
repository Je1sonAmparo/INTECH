using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTECH.Models;

public class Servicios
{
    [Key]
    public int ServicioId { get; set; }

    public bool IsDeleted { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
    public int CategoriaServicioId { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    [ForeignKey("CategoriaServicioId")]
    [InverseProperty("Servicios")]
    public virtual CategoriasServicios CategoriaServicio { get; set; } = null!;

    [InverseProperty("Servicio")]
    public virtual ICollection<SolicitudesDetalle> SolicitudesDetalle { get; set; } = new List<SolicitudesDetalle>();

    [InverseProperty("Servicio")]
    public virtual ICollection<LevantamientosDetalle> LevantamientosDetalle { get; set; } = new List<LevantamientosDetalle>();
}
