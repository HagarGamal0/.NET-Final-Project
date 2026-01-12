using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class MakePackageCustomerNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Destination",
                table: "Packages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "Packages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");



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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AlterColumn<string>(
                name: "Destination",
                table: "Packages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
