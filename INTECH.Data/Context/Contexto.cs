using Microsoft.EntityFrameworkCore;
using INTECH.Models;

namespace INTECH.Context;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {
    }

    public DbSet<EstadosSolicitudes> EstadosSolicitudes { get; set; }
    public DbSet<Solicitudes> Solicitudes { get; set; }
    public DbSet<SolicitudesDetalle> SolicitudesDetalle { get; set; }
    public DbSet<CategoriasServicios> CategoriasServicios { get; set; }
    public DbSet<Servicios> Servicios { get; set; }
    public DbSet<Clientes> Clientes { get; set; }
    public DbSet<TelefonosClientes> TelefonosClientes { get; set; }
    public DbSet<Levantamientos> Levantamientos { get; set; }
    public DbSet<FotosLevantamientos> FotosLevantamientos { get; set; }
    public DbSet<Agendas> Agendas { get; set; }
    public DbSet<OrdenesTrabajo> OrdenesTrabajo { get; set; }
    public DbSet<Historiales> Historiales { get; set; }
    public DbSet<HistorialesDetalle> HistorialesDetalle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EstadosSolicitudes>().HasData(
            new EstadosSolicitudes
            {
                EstadoSolicitudId = 1,
                Nombre = "Pendiente"
            },
            new EstadosSolicitudes
            {
                EstadoSolicitudId = 2,
                Nombre = "En proceso"
            },
            new EstadosSolicitudes
            {
                EstadoSolicitudId = 3,
                Nombre = "En Espera Administración"
            },
            new EstadosSolicitudes
            {
                EstadoSolicitudId = 4,
                Nombre = "En revisión"
            },
            new EstadosSolicitudes
            {
                EstadoSolicitudId = 5,
                Nombre = "Enviada al cliente"
            },
            new EstadosSolicitudes
            {
                EstadoSolicitudId = 6,
                Nombre = "Aprobada"
            },
            new EstadosSolicitudes
            {
                EstadoSolicitudId = 7,
                Nombre = "Rechazada"
            },
            new EstadosSolicitudes
            {
                EstadoSolicitudId = 8,
                Nombre = "Pendiente de cobro"
            },
            new EstadosSolicitudes
            {
                EstadoSolicitudId = 9,
                Nombre = "Finalizada"
            },
            new EstadosSolicitudes
            {
                EstadoSolicitudId = 10,
                Nombre = "Cancelada"
            }
        );

        modelBuilder.Entity<CategoriasServicios>().HasData(
            new CategoriasServicios
            {
                CategoriaServicioId = 6,
                Nombre = "Instalaciones"
            },
            new CategoriasServicios
            {
                CategoriaServicioId = 7,
                Nombre = "Mantenimiento"
            }
        );

        modelBuilder.Entity<Servicios>().HasData(
            new Servicios
            {
                ServicioId = 1,
                CategoriaServicioId = 6,
                Nombre = "Instalación de Paneles Solares",
                Activo = true
            },
            new Servicios
            {
                ServicioId = 2,
                CategoriaServicioId = 6,
                Nombre = "Instalación de Sistema UPS",
                Activo = true
            },
            new Servicios
            {
                ServicioId = 3,
                CategoriaServicioId = 7,
                Nombre = "Mantenimiento Eléctrico",
                Activo = true
            },
            new Servicios
            {
                ServicioId = 4,
                CategoriaServicioId = 6,
                Nombre = "Instalación de Planta Eléctrica",
                Activo = true
            }
        );
    }
}
