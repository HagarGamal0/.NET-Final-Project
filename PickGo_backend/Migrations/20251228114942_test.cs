using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryOTP",
                table: "Packages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OTPVerified",
                table: "Packages",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryOTP",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "OTPVerified",
                table: "Packages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7caf19a9-8a2a-4610-a1ba-e7cc0827037c", "AQAAAAIAAYagAAAAEKGLOdtGwEKXyV0AJPFPlJoiHkVJGrcCHo8UJn8327NnWWcd+O80Y2TefimMRklsxw==", "87886765-8e01-43dd-b11f-dd16d3f9ad7a" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 17, 7, 30, 56, DateTimeKind.Utc).AddTicks(5856));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 17, 7, 30, 56, DateTimeKind.Utc).AddTicks(5864));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 17, 7, 30, 56, DateTimeKind.Utc).AddTicks(5870));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 17, 7, 30, 56, DateTimeKind.Utc).AddTicks(5873));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 17, 7, 30, 56, DateTimeKind.Utc).AddTicks(5875));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 17, 7, 30, 56, DateTimeKind.Utc).AddTicks(5878));
        }
    }
}
