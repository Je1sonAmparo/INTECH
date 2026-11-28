using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace INTECH.Migrations;

/// <inheritdoc />
public partial class _20261128002853_AgregarServiciosIniciales : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "CategoriasServicios",
            columns: new[] { "CategoriaServicioId", "Nombre" },
            values: new object[,]
            {
                { 6, "Instalaciones" },
                { 7, "Mantenimiento" }
            });

        migrationBuilder.InsertData(
            table: "Servicios",
            columns: new[] { "ServicioId", "Activo", "CategoriaServicioId", "IsDeleted", "Nombre" },
            values: new object[,]
            {
                { 1, true, 6, false, "Instalación de Paneles Solares" },
                { 2, true, 6, false, "Instalación de Sistema UPS" },
                { 3, true, 7, false, "Mantenimiento Eléctrico" },
                { 4, true, 6, false, "Instalación de Planta Eléctrica" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Servicios",
            keyColumn: "ServicioId",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "Servicios",
            keyColumn: "ServicioId",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "Servicios",
            keyColumn: "ServicioId",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "Servicios",
            keyColumn: "ServicioId",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "CategoriasServicios",
            keyColumn: "CategoriaServicioId",
            keyValue: 6);

        migrationBuilder.DeleteData(
            table: "CategoriasServicios",
            keyColumn: "CategoriaServicioId",
            keyValue: 7);
    }
}
