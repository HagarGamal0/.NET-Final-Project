using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class dsnsssssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courier_CourierSubscription_CurrentSubscriptionId",
                table: "Courier");

            migrationBuilder.DropForeignKey(
                name: "FK_CourierSubscription_Courier_CourierId",
                table: "CourierSubscription");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8636c87-177f-4a32-8b7c-80760ef359b7", "AQAAAAIAAYagAAAAEE3QFSzet0RWwrRBRKdCqvCjYlK1VRqUkdsioBm4KuYev1DrEDyuGPCy3byh614GSA==", "608ffeb1-989e-4541-b742-ad924f65d93c" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 9, 53, 881, DateTimeKind.Utc).AddTicks(3669));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 9, 53, 881, DateTimeKind.Utc).AddTicks(3674));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 9, 53, 881, DateTimeKind.Utc).AddTicks(3678));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 9, 53, 881, DateTimeKind.Utc).AddTicks(3679));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 9, 53, 881, DateTimeKind.Utc).AddTicks(3680));

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 17, 15, 9, 53, 881, DateTimeKind.Utc).AddTicks(3682));

            migrationBuilder.AddForeignKey(
                name: "FK_Courier_CourierSubscription_CurrentSubscriptionId",
                table: "Courier",
                column: "CurrentSubscriptionId",
                principalTable: "CourierSubscription",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourierSubscription_Courier_CourierId",
                table: "CourierSubscription",
                column: "CourierId",
                principalTable: "Courier",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courier_CourierSubscription_CurrentSubscriptionId",
                table: "Courier");

            migrationBuilder.DropForeignKey(
                name: "FK_CourierSubscription_Courier_CourierId",
                table: "CourierSubscription");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Courier_CourierSubscription_CurrentSubscriptionId",
                table: "Courier",
                column: "CurrentSubscriptionId",
                principalTable: "CourierSubscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CourierSubscription_Courier_CourierId",
                table: "CourierSubscription",
                column: "CourierId",
                principalTable: "Courier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
