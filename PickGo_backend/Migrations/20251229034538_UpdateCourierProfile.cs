using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourierProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "MaxWeight",
                table: "Courier",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "LicenseNumber",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "Courier");

            migrationBuilder.AlterColumn<float>(
                name: "MaxWeight",
                table: "Courier",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenseNumber",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6ee60593-e322-4df7-bc89-8540b43a4aae", "AQAAAAIAAYagAAAAEGJ1Jpg410OTq5RBHQmm4Cp1QHRdtVOz1WbD8Q1Oc3ua+N9qCWmFfQexhchJlZZauw==", "5d797186-c651-4865-ac38-b785d0fc2b75" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 2, 2, 14, 425, DateTimeKind.Utc).AddTicks(5382));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 2, 2, 14, 425, DateTimeKind.Utc).AddTicks(5388));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 2, 2, 14, 425, DateTimeKind.Utc).AddTicks(5392));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 2, 2, 14, 425, DateTimeKind.Utc).AddTicks(5394));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 2, 2, 14, 425, DateTimeKind.Utc).AddTicks(5397));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 29, 2, 2, 14, 425, DateTimeKind.Utc).AddTicks(5399));
        }
    }
}
