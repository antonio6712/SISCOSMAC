using Microsoft.EntityFrameworkCore.Migrations;

namespace SISCOSMAC.DAL.Migrations
{
    public partial class actualzacionFkSolicitud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdenTrabajo_Usuario_SolicitudId",
                table: "OrdenTrabajo");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenTrabajo_SolicitudMantenimientoCorrectivo_SolicitudId",
                table: "OrdenTrabajo",
                column: "SolicitudId",
                principalTable: "SolicitudMantenimientoCorrectivo",
                principalColumn: "SolicitudId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdenTrabajo_SolicitudMantenimientoCorrectivo_SolicitudId",
                table: "OrdenTrabajo");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenTrabajo_Usuario_SolicitudId",
                table: "OrdenTrabajo",
                column: "SolicitudId",
                principalTable: "Usuario",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
