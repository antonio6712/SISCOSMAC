using Microsoft.EntityFrameworkCore.Migrations;

namespace SISCOSMAC.DAL.Migrations
{
    public partial class _2Campos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enviado",
                table: "OrdenTrabajo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Recibido",
                table: "OrdenTrabajo",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enviado",
                table: "OrdenTrabajo");

            migrationBuilder.DropColumn(
                name: "Recibido",
                table: "OrdenTrabajo");
        }
    }
}
