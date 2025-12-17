using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class dsnssssssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "CourierSubscription",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourierSubscription",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "CourierSubscription",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CurrentSubscriptionId",
                table: "Courier",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2e528be0-8f5c-475a-91bb-7619ff598559", "AQAAAAIAAYagAAAAEEpl/w8Zlp/xNrT8xH43dSADFxTVArvcbiG3dLw+KU0QDqLORXBMq6CK/ftpkmgDsQ==", "b20d6c26-4929-404f-9f1c-739921710fd3" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 3, 47, 433, DateTimeKind.Utc).AddTicks(4085));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 3, 47, 433, DateTimeKind.Utc).AddTicks(4090));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 3, 47, 433, DateTimeKind.Utc).AddTicks(4092));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 3, 47, 433, DateTimeKind.Utc).AddTicks(4094));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 3, 47, 433, DateTimeKind.Utc).AddTicks(4095));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 3, 47, 433, DateTimeKind.Utc).AddTicks(4097));

            migrationBuilder.CreateIndex(
                name: "IX_Courier_CurrentSubscriptionId",
                table: "Courier",
                column: "CurrentSubscriptionId",
                unique: true,
                filter: "[CurrentSubscriptionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Courier_CourierSubscription_CurrentSubscriptionId",
                table: "Courier",
                column: "CurrentSubscriptionId",
                principalTable: "CourierSubscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courier_CourierSubscription_CurrentSubscriptionId",
                table: "Courier");

            migrationBuilder.DropIndex(
                name: "IX_Courier_CurrentSubscriptionId",
                table: "Courier");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "CourierSubscription");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourierSubscription");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "CourierSubscription");

            migrationBuilder.DropColumn(
                name: "CurrentSubscriptionId",
                table: "Courier");

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
    }
}
