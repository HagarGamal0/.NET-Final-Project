using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class jgggssssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VehicleType",
                table: "Courier",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VehicleType",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2db26660-8423-47ba-b9de-d673991213cc", "AQAAAAIAAYagAAAAEPGwzsxhojmUN/CBnnFw1FeVzthy1YmUz2gPTdpHbUWV0l887H4rGGz5ITpFkQ/5bw==", "2e9c4bc7-717d-4aba-83b1-bafea24f3bab" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 16, 40, 32, 709, DateTimeKind.Utc).AddTicks(5410));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 16, 40, 32, 709, DateTimeKind.Utc).AddTicks(5417));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 16, 40, 32, 709, DateTimeKind.Utc).AddTicks(5421));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 16, 40, 32, 709, DateTimeKind.Utc).AddTicks(5423));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 16, 40, 32, 709, DateTimeKind.Utc).AddTicks(5425));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 20, 16, 40, 32, 709, DateTimeKind.Utc).AddTicks(5427));
        }
    }
}
