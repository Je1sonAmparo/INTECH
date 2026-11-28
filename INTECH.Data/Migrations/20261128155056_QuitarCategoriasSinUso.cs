using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace INTECH.Migrations;

/// <inheritdoc />
public partial class _20261128155056_QuitarCategoriasSinUso : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "CategoriasServicios",
            keyColumn: "CategoriaServicioId",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "CategoriasServicios",
            keyColumn: "CategoriaServicioId",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "CategoriasServicios",
            keyColumn: "CategoriaServicioId",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "CategoriasServicios",
            keyColumn: "CategoriaServicioId",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "CategoriasServicios",
            keyColumn: "CategoriaServicioId",
            keyValue: 5);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
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
    }
}
