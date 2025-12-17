using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class dsndk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReadyForPickup",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Courier",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d6f083bf-5313-4c17-81be-fd70d8b9dbb6", "AQAAAAIAAYagAAAAEBX55hj9v6euQjIwEdOrq0dhBEAihOdW2ttTPZsw6/+we+j6qtFhd2TSD+lKEtquXg==", "d094d727-ddbf-4132-960c-dc649e185d6d" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 13, 10, 27, 921, DateTimeKind.Utc).AddTicks(9298));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 13, 10, 27, 921, DateTimeKind.Utc).AddTicks(9302));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 13, 10, 27, 921, DateTimeKind.Utc).AddTicks(9306));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 13, 10, 27, 921, DateTimeKind.Utc).AddTicks(9308));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 13, 10, 27, 921, DateTimeKind.Utc).AddTicks(9310));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 13, 10, 27, 921, DateTimeKind.Utc).AddTicks(9311));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadyForPickup",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Courier");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
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
    }
}
