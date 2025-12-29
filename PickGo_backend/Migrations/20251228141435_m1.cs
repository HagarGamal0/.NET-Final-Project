using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "793982d1-c53e-4bc8-96ed-4ffbcda62585", "AQAAAAIAAYagAAAAEMtOXSSeWn2JCX3Bv3M+o9tb8N/iwPui5EIiVKEotTZ+3++PydQNwVyg6HAv1Jij1g==", "e5fc51b9-c076-4089-bcb6-dc9d4ddb5b15" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 33, 436, DateTimeKind.Utc).AddTicks(5652));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 33, 436, DateTimeKind.Utc).AddTicks(5663));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 33, 436, DateTimeKind.Utc).AddTicks(5669));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 33, 436, DateTimeKind.Utc).AddTicks(5671));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 33, 436, DateTimeKind.Utc).AddTicks(5674));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 33, 436, DateTimeKind.Utc).AddTicks(5676));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "725ab3b6-948b-4fca-adf5-68264087ca18", "AQAAAAIAAYagAAAAEBs2PTDjy+H5rg98ZG5V31eSiMmL29FHYG4RFKIE3awAFoAs3sAatkXdAfjkooQ7HQ==", "6bcf6b99-6bcd-4b2b-bfb0-38ee5d556efb" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 11, 49, 40, 638, DateTimeKind.Utc).AddTicks(8937));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 11, 49, 40, 638, DateTimeKind.Utc).AddTicks(8943));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 11, 49, 40, 638, DateTimeKind.Utc).AddTicks(8950));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 11, 49, 40, 638, DateTimeKind.Utc).AddTicks(8953));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 11, 49, 40, 638, DateTimeKind.Utc).AddTicks(8956));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 11, 49, 40, 638, DateTimeKind.Utc).AddTicks(8959));
        }
    }
}
