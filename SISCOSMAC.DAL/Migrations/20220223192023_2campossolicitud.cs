using Microsoft.EntityFrameworkCore.Migrations;

namespace SISCOSMAC.DAL.Migrations
{
    public partial class _2campossolicitud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enviado",
                table: "SolicitudMantenimientoCorrectivo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Recibido",
                table: "SolicitudMantenimientoCorrectivo",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enviado",
                table: "SolicitudMantenimientoCorrectivo");

            migrationBuilder.DropColumn(
                name: "Recibido",
                table: "SolicitudMantenimientoCorrectivo");
        }
    }
}
