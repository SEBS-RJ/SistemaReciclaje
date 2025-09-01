using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaReciclaje.Migrations
{
    /// <inheritdoc />
    public partial class act002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuariosSistema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosSistema", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "UsuariosSistema",
                columns: new[] { "Id", "Activo", "Email", "FechaCreacion", "NombreCompleto", "NombreUsuario", "PasswordHash", "Rol", "UltimoAcceso" },
                values: new object[] { 1, true, "admin@reciclaje.com", new DateTime(2025, 9, 1, 1, 28, 30, 803, DateTimeKind.Local).AddTicks(2660), "Administrador del Sistema", "admin", "$2a$11$j5vxnOqZktRcwhPN0Vc3CejFqPiviDuWC2/vL/4bQjx4HBRI9XLGq", 1, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuariosSistema");
        }
    }
}
