using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletedDeliveries",
                table: "Courier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "86cea5b0-ba58-40f1-a33b-ec4e4af34fa3", "AQAAAAIAAYagAAAAEMczLQZDUpdxmD5ej3YuTmh3v7exYOBmEhDvxX4KrxZbxYA12d0TDUTp7MH1NQaZhg==", "2bb10fff-f6d6-4516-bdaf-05ffa3fa37ac" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2564));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2569));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2574));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2576));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2579));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2581));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDeliveries",
                table: "Courier");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "458922c5-6fb0-480c-8291-f05f0237e36f", "AQAAAAIAAYagAAAAEALmqH1qQi+9zBQhcPlDXetv2x0axN3jn5OugVqo5fBFG9Fs4t1smzo7Y+7ofh6XWg==", "f0a0f50f-fb53-4959-8b74-4f6d2e552627" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 3, 45, 37, 467, DateTimeKind.Utc).AddTicks(8771));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 3, 45, 37, 467, DateTimeKind.Utc).AddTicks(8778));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 3, 45, 37, 467, DateTimeKind.Utc).AddTicks(8783));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 3, 45, 37, 467, DateTimeKind.Utc).AddTicks(8785));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 3, 45, 37, 467, DateTimeKind.Utc).AddTicks(8787));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 3, 45, 37, 467, DateTimeKind.Utc).AddTicks(8789));
        }
    }
}
