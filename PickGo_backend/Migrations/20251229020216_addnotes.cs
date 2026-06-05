using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class addnotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "deliveryProofs",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "deliveryProofs");

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
    }
}
