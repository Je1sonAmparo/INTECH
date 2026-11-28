using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INTECH.Migrations;

/// <inheritdoc />
public partial class _20261128184456_AgregarDetalleLevantamientos : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_FotosLevantamientos_Levantamientos_LevantamientoId",
            table: "FotosLevantamientos");

        migrationBuilder.DropColumn(
            name: "CargasElectricas",
            table: "Levantamientos");

        migrationBuilder.DropColumn(
            name: "DisponibilidadEspaciosFisicos",
            table: "Levantamientos");

        migrationBuilder.DropColumn(
            name: "EstabilidadTension",
            table: "Levantamientos");

        migrationBuilder.DropColumn(
            name: "RecorridoCableado",
            table: "Levantamientos");

        migrationBuilder.DropColumn(
            name: "RecorridoTuberias",
            table: "Levantamientos");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}