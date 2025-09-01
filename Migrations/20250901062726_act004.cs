
 using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaReciclaje.Migrations
{
    /// <inheritdoc />
    public partial class act004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UsuariosSistema",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 1, 2, 27, 25, 566, DateTimeKind.Local).AddTicks(7258), "$2a$11$s5qeyjTaYiKL9EvZusju.Oz9W2kcJiDtvpJaDLzXO8v5Jg5xTXt0y" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UsuariosSistema",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 1, 1, 37, 39, 187, DateTimeKind.Local).AddTicks(271), "$2a$11$afUbHa.V0/Rc3DjZrUPNa.upQqK/AsNKhSbGOHsz5KCyY9mQaLYN2" });
        }
    }
}
