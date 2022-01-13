using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace SISCOSMAC.DAL.Migrations
{
    public partial class addHashSal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    DepartamentoId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NombreDepartamento = table.Column<string>(nullable: false),
                    OrdenTrabajo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.DepartamentoId);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: false),
                    APaterno = table.Column<string>(nullable: false),
                    AMaterno = table.Column<string>(nullable: false),
                    UsuarioLogin = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<byte[]>(maxLength: 32, nullable: false),
                    PasswordSal = table.Column<byte[]>(maxLength: 32, nullable: false),
                    Rol = table.Column<string>(nullable: false),
                    DepartamentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Usuario_Departamento_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamento",
                        principalColumn: "DepartamentoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudMantenimientoCorrectivo",
                columns: table => new
                {
                    SolicitudId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Folio = table.Column<int>(nullable: true),
                    DepartamentoDirigido = table.Column<string>(nullable: false),
                    AreaSolicitante = table.Column<string>(nullable: false),
                    NombreSolicitante = table.Column<string>(nullable: false),
                    FechaElaboracion = table.Column<DateTime>(nullable: false),
                    DescripcionServicios = table.Column<string>(nullable: false),
                    UsuarioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudMantenimientoCorrectivo", x => x.SolicitudId);
                    table.ForeignKey(
                        name: "FK_SolicitudMantenimientoCorrectivo_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudMantenimientoCorrectivo_UsuarioId",
                table: "SolicitudMantenimientoCorrectivo",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_DepartamentoId",
                table: "Usuario",
                column: "DepartamentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudMantenimientoCorrectivo");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Departamento");
        }
    }
}
