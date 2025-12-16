using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class _5545465 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "customers",
                newName: "PhoneNumber");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a8a75b6b-5bb6-49e2-bae5-d0fcef123a81", "AQAAAAIAAYagAAAAEMXVgwopFuk9VMm5Am3myCXfUjLKMha0gfQcvi4uI9ifmnptNFER4OVPHtvE5HeyTA==", "307bc26e-19ab-4f2c-b810-1931966234f2" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 12, 30, 3, 173, DateTimeKind.Utc).AddTicks(2093));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 12, 30, 3, 173, DateTimeKind.Utc).AddTicks(2098));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 12, 30, 3, 173, DateTimeKind.Utc).AddTicks(2101));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 12, 30, 3, 173, DateTimeKind.Utc).AddTicks(2103));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 12, 30, 3, 173, DateTimeKind.Utc).AddTicks(2104));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 12, 30, 3, 173, DateTimeKind.Utc).AddTicks(2105));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "customers",
                newName: "Address");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7246f485-8ad2-440c-ac17-bf3df3f73b59", "AQAAAAIAAYagAAAAEGEOkB+aWsbqLi+oKUUy4U0n+ps35hLYLJnjyxKwpEvbKFPgjcPdNXmJ7qo52JVOLw==", "dbfd714d-dbdb-4108-9b27-b9766092cbac" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 11, 13, 51, 493, DateTimeKind.Utc).AddTicks(4241));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 11, 13, 51, 493, DateTimeKind.Utc).AddTicks(4243));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 11, 13, 51, 493, DateTimeKind.Utc).AddTicks(4246));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 11, 13, 51, 493, DateTimeKind.Utc).AddTicks(4247));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 11, 13, 51, 493, DateTimeKind.Utc).AddTicks(4248));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 11, 13, 51, 493, DateTimeKind.Utc).AddTicks(4249));
        }
    }
}
