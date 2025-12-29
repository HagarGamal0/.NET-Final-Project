using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdPhotoUrl",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "22c23550-386a-44c1-88c7-a06c906d0102", "AQAAAAIAAYagAAAAEJTEqIewflDkv1ttRynHEyFxR5NPaeBSsSYig+8qLhw4MZUCeDlP/J3nX9bgtdUJ1A==", "4aa004cf-6dd4-425a-812c-de0828dec72b" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 59, 482, DateTimeKind.Utc).AddTicks(5164));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 59, 482, DateTimeKind.Utc).AddTicks(5172));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 59, 482, DateTimeKind.Utc).AddTicks(5176));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 59, 482, DateTimeKind.Utc).AddTicks(5179));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 59, 482, DateTimeKind.Utc).AddTicks(5262));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 14, 59, 482, DateTimeKind.Utc).AddTicks(5265));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPhotoUrl",
                table: "Courier");

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
    }
}
