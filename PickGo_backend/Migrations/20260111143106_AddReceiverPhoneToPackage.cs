using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiverPhoneToPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverPhone",
                table: "Packages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a35329e5-b435-4002-b540-ee48d126e912", "AQAAAAIAAYagAAAAEI57g1zhtuLRhtDlzNvh/xHAEf7DJBL8CPkNErorVVNcgHo+6sV92dTwEwW9eoXoBw==", "0a6541a0-d75a-4f0a-b865-4556f927d166" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 14, 31, 5, 195, DateTimeKind.Utc).AddTicks(7767));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 14, 31, 5, 195, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 14, 31, 5, 195, DateTimeKind.Utc).AddTicks(7781));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 14, 31, 5, 195, DateTimeKind.Utc).AddTicks(7783));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 14, 31, 5, 195, DateTimeKind.Utc).AddTicks(7786));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 14, 31, 5, 195, DateTimeKind.Utc).AddTicks(7789));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverPhone",
                table: "Packages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "61e68bec-7d14-4eb7-a282-ba0f7caa36c2", "AQAAAAIAAYagAAAAEEfp9yyquVKnJfqnGSWw2KF+xwKt+5RH7bTvN76LBXgOsZGc4tcAo3sTmEX8KX4xXg==", "cbec4825-d98e-4035-9aa7-aef27734fb2a" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 11, 37, 56, 768, DateTimeKind.Utc).AddTicks(7281));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 11, 37, 56, 768, DateTimeKind.Utc).AddTicks(7286));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 11, 37, 56, 768, DateTimeKind.Utc).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 11, 37, 56, 768, DateTimeKind.Utc).AddTicks(7291));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 11, 37, 56, 768, DateTimeKind.Utc).AddTicks(7293));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 11, 11, 37, 56, 768, DateTimeKind.Utc).AddTicks(7294));
        }
    }
}
