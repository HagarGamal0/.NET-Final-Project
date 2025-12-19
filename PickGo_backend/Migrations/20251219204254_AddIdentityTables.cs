using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d79e0025-2e91-412b-bbd2-5198d54521b6", "AQAAAAIAAYagAAAAEFLv/mYycB4bM92E09JiwwUij1FEZbTHUTgd65F8lGGdTNt07GQm+vMGoG6qnFeYsA==", "002c45ce-bf5c-4ee3-9a15-b843b267542e" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 20, 42, 49, 37, DateTimeKind.Utc).AddTicks(5108));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 20, 42, 49, 37, DateTimeKind.Utc).AddTicks(5115));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 20, 42, 49, 37, DateTimeKind.Utc).AddTicks(5121));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 20, 42, 49, 37, DateTimeKind.Utc).AddTicks(5123));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 20, 42, 49, 37, DateTimeKind.Utc).AddTicks(5126));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 20, 42, 49, 37, DateTimeKind.Utc).AddTicks(5128));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ddf5cd42-3a9e-485b-9767-a40e27f47c62", "AQAAAAIAAYagAAAAEOnDV9pYb0/AqIpSXVtsg7sMEmHVML5gofS4WT7xUbIp+eETsQsLUmJTOLcuVXGbnA==", "674a5aaa-46f1-483b-8db6-2388299e208f" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 14, 16, 32, 743, DateTimeKind.Utc).AddTicks(1358));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 14, 16, 32, 743, DateTimeKind.Utc).AddTicks(1364));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 14, 16, 32, 743, DateTimeKind.Utc).AddTicks(1371));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 14, 16, 32, 743, DateTimeKind.Utc).AddTicks(1373));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 14, 16, 32, 743, DateTimeKind.Utc).AddTicks(1375));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 19, 14, 16, 32, 743, DateTimeKind.Utc).AddTicks(1377));
        }
    }
}
