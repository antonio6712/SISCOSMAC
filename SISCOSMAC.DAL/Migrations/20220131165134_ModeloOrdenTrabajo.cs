using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace SISCOSMAC.DAL.Migrations
{
    public partial class ModeloOrdenTrabajo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdenTrabajo",
                columns: table => new
                {
                    OrdenId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Folio = table.Column<int>(nullable: false),
                    Mantenimiento = table.Column<string>(nullable: false),
                    TipoServicio = table.Column<string>(nullable: false),
                    Asignado = table.Column<string>(nullable: false),
                    FechaRealizacion = table.Column<DateTime>(nullable: false),
                    TrabajoRealizado = table.Column<string>(nullable: false),
                    VerificadoLiberado = table.Column<string>(nullable: false),
                    AprobadoPor = table.Column<string>(nullable: false),
                    SolicitudId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenTrabajo", x => x.OrdenId);
                    table.ForeignKey(
                        name: "FK_OrdenTrabajo_Usuario_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdenTrabajo_SolicitudId",
                table: "OrdenTrabajo",
                column: "SolicitudId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdenTrabajo");
        }
    }
}
