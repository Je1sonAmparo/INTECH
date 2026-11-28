using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTECH.Models;

public class CategoriasServicios
{
    [Key]
    public int CategoriaServicioId { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    [StringLength(80, ErrorMessage = "El nombre no puede superar los 80 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [InverseProperty("CategoriaServicio")]
    public virtual ICollection<Servicios> Servicios { get; set; } = new List<Servicios>();
}
