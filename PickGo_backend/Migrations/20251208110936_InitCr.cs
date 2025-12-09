using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitCr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "94bfc362-6237-4f83-8eee-1c528eef0871", "AQAAAAIAAYagAAAAEP2t9nGM8E+FdV5gJ+UkBFU+oI61KViRHoeXMt8U5cYBt4P5x3UBl1ZqFkxxPTRPBA==", "33b8c2e1-6c25-4f6c-9567-df7673e2fe63" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c9b07bb-3cd1-4163-bc6a-89bcf41fc901", "AQAAAAIAAYagAAAAEDrbSFFHku7klWSgLj1e9EjIbimonEagL6EhxjVQYbh2BuULfdbqRXBmI8PHGhUMhw==", "925c3d88-accc-4296-968c-6e5eff61a10c" });
        }
    }
}
