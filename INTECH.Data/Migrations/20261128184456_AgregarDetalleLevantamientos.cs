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

        migrationBuilder.RenameColumn(
            name: "LevantamientoId",
            table: "FotosLevantamientos",
            newName: "DetalleId");

        migrationBuilder.RenameIndex(
            name: "IX_FotosLevantamientos_LevantamientoId",
            table: "FotosLevantamientos",
            newName: "IX_FotosLevantamientos_DetalleId");

        migrationBuilder.AddColumn<int>(
            name: "BreakerPrincipal",
            table: "Levantamientos",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "ConsumoMensual",
            table: "Levantamientos",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "TensionMedida",
            table: "Levantamientos",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "LevantamientosDetalle",
            columns: table => new
            {
                DetalleId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                LevantamientoId = table.Column<int>(type: "int", nullable: false),
                ServicioId = table.Column<int>(type: "int", nullable: false),
                Ubicacion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                DistanciaRecorrido = table.Column<double>(type: "float", nullable: false),
                EspacioDisponible = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                DescripcionTecnica = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                Viable = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LevantamientosDetalle", x => x.DetalleId);
                table.ForeignKey(
                    name: "FK_LevantamientosDetalle_Levantamientos_LevantamientoId",
                    column: x => x.LevantamientoId,
                    principalTable: "Levantamientos",
                    principalColumn: "LevantamientoId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_LevantamientosDetalle_Servicios_ServicioId",
                    column: x => x.ServicioId,
                    principalTable: "Servicios",
                    principalColumn: "ServicioId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_LevantamientosDetalle_LevantamientoId",
            table: "LevantamientosDetalle",
            column: "LevantamientoId");

        migrationBuilder.CreateIndex(
            name: "IX_LevantamientosDetalle_ServicioId",
            table: "LevantamientosDetalle",
            column: "ServicioId");

        migrationBuilder.AddForeignKey(
            name: "FK_FotosLevantamientos_LevantamientosDetalle_DetalleId",
            table: "FotosLevantamientos",
            column: "DetalleId",
            principalTable: "LevantamientosDetalle",
            principalColumn: "DetalleId",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_FotosLevantamientos_LevantamientosDetalle_DetalleId",
            table: "FotosLevantamientos");

        migrationBuilder.DropTable(
            name: "LevantamientosDetalle");

        migrationBuilder.DropColumn(
            name: "BreakerPrincipal",
            table: "Levantamientos");

        migrationBuilder.DropColumn(
            name: "ConsumoMensual",
            table: "Levantamientos");

        migrationBuilder.DropColumn(
            name: "TensionMedida",
            table: "Levantamientos");

        migrationBuilder.RenameColumn(
            name: "DetalleId",
            table: "FotosLevantamientos",
            newName: "LevantamientoId");

        migrationBuilder.RenameIndex(
            name: "IX_FotosLevantamientos_DetalleId",
            table: "FotosLevantamientos",
            newName: "IX_FotosLevantamientos_LevantamientoId");

        migrationBuilder.AddColumn<string>(
            name: "CargasElectricas",
            table: "Levantamientos",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "DisponibilidadEspaciosFisicos",
            table: "Levantamientos",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "EstabilidadTension",
            table: "Levantamientos",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "RecorridoCableado",
            table: "Levantamientos",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "RecorridoTuberias",
            table: "Levantamientos",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddForeignKey(
            name: "FK_FotosLevantamientos_Levantamientos_LevantamientoId",
            table: "FotosLevantamientos",
            column: "LevantamientoId",
            principalTable: "Levantamientos",
            principalColumn: "LevantamientoId",
            onDelete: ReferentialAction.Cascade);
    }
}
