using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaReciclaje.Migrations
{
    /// <inheritdoc />
    public partial class act003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UsuariosSistema",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 1, 1, 37, 39, 187, DateTimeKind.Local).AddTicks(271), "$2a$11$afUbHa.V0/Rc3DjZrUPNa.upQqK/AsNKhSbGOHsz5KCyY9mQaLYN2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UsuariosSistema",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 1, 1, 28, 30, 803, DateTimeKind.Local).AddTicks(2660), "$2a$11$j5vxnOqZktRcwhPN0Vc3CejFqPiviDuWC2/vL/4bQjx4HBRI9XLGq" });
        }
    }
}
