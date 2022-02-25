using Microsoft.EntityFrameworkCore.Migrations;

namespace SISCOSMAC.DAL.Migrations
{
    public partial class retirarcampos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enviado",
                table: "OrdenTrabajo");

            migrationBuilder.DropColumn(
                name: "Recibido",
                table: "OrdenTrabajo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enviado",
                table: "OrdenTrabajo",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Recibido",
                table: "OrdenTrabajo",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
