using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace INTECH.Migrations;

/// <inheritdoc />
public partial class _20261128001225_AgregarServiciosYSolicitudesDetalle : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CategoriasServicios",
            columns: table => new
            {
                CategoriaServicioId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CategoriasServicios", x => x.CategoriaServicioId);
            });

        migrationBuilder.CreateTable(
            name: "Servicios",
            columns: table => new
            {
                ServicioId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CategoriaServicioId = table.Column<int>(type: "int", nullable: false),
                Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Activo = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Servicios", x => x.ServicioId);
                table.ForeignKey(
                    name: "FK_Servicios_CategoriasServicios_CategoriaServicioId",
                    column: x => x.CategoriaServicioId,
                    principalTable: "CategoriasServicios",
                    principalColumn: "CategoriaServicioId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SolicitudesDetalle",
            columns: table => new
            {
                DetalleId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                SolicitudId = table.Column<int>(type: "int", nullable: false),
                ServicioId = table.Column<int>(type: "int", nullable: false),
                Cantidad = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SolicitudesDetalle", x => x.DetalleId);
                table.ForeignKey(
                    name: "FK_SolicitudesDetalle_Servicios_ServicioId",
                    column: x => x.ServicioId,
                    principalTable: "Servicios",
                    principalColumn: "ServicioId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_SolicitudesDetalle_Solicitudes_SolicitudId",
                    column: x => x.SolicitudId,
                    principalTable: "Solicitudes",
                    principalColumn: "SolicitudId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "CategoriasServicios",
            columns: new[] { "CategoriaServicioId", "Nombre" },
            values: new object[,]
            {
                { 1, "Energía solar" },
                { 2, "Climatización" },
                { 3, "Instalaciones eléctricas" },
                { 4, "Respaldo eléctrico" },
                { 5, "Seguridad electrónica" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Servicios_CategoriaServicioId",
            table: "Servicios",
            column: "CategoriaServicioId");

        migrationBuilder.CreateIndex(
            name: "IX_SolicitudesDetalle_ServicioId",
            table: "SolicitudesDetalle",
            column: "ServicioId");

        migrationBuilder.CreateIndex(
            name: "IX_SolicitudesDetalle_SolicitudId",
            table: "SolicitudesDetalle",
            column: "SolicitudId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SolicitudesDetalle");

        migrationBuilder.DropTable(
            name: "Servicios");

        migrationBuilder.DropTable(
            name: "CategoriasServicios");
    }
}
