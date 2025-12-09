using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class _111111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e908b58f-7a40-4a32-81ba-cb9e62b2607d", "AQAAAAIAAYagAAAAEBxsAmTFz/R/607KXGZr/IM1AcKzQ0nC1msWbLnAG50aKk2OygtUl8pdvqHiqx882g==", "2ccaca21-4e1c-45a1-b7d6-51a3a00f5caf" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5dca1eb3-0fc5-455c-a495-298953cf9a5d", "AQAAAAIAAYagAAAAEIxY71Qo9fDa9nwaMed3Pl+BU7CNuaaFyNIFxsrbk5791FpvuICXmrxjVKGBuTIyww==", "4066f232-209c-4e82-a711-a81cece973c1" });
        }
    }
}
