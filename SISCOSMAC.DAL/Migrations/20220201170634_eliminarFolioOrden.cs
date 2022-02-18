using Microsoft.EntityFrameworkCore.Migrations;

namespace SISCOSMAC.DAL.Migrations
{
    public partial class eliminarFolioOrden : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Folio",
                table: "OrdenTrabajo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Folio",
                table: "OrdenTrabajo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
