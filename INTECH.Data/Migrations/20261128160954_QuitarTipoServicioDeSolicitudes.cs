using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INTECH.Migrations;

/// <inheritdoc />
public partial class _20261128160954_QuitarTipoServicioDeSolicitudes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"INSERT INTO SolicitudesDetalle (SolicitudId, ServicioId, Cantidad)
            SELECT s.SolicitudId, se.ServicioId, 1
            FROM Solicitudes s
            INNER JOIN Servicios se ON se.Nombre = s.TipoServicio
            WHERE NOT EXISTS (SELECT 1 FROM SolicitudesDetalle d WHERE d.SolicitudId = s.SolicitudId)");

        migrationBuilder.DropColumn(
            name: "TipoServicio",
            table: "Solicitudes");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "TipoServicio",
            table: "Solicitudes",
            type: "nvarchar(150)",
            maxLength: 150,
            nullable: false,
            defaultValue: "");
    }
}
