using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class addSupscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "MaxOrders",
                table: "Subscription");

            migrationBuilder.AddColumn<bool>(
                name: "IsOneTimePayment",
                table: "Subscription",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUnlimited",
                table: "Subscription",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxTrips",
                table: "Subscription",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "CourierSubscription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsedTrips",
                table: "CourierSubscription",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "753aefc3-0ba3-4eb6-aa5f-e776f2383b00", "AQAAAAIAAYagAAAAEPqX+oQsa764ZFv8ltFh3OQZFaLtvBmMs6zZAYTxpWgla9CH0CJLFdSw59H4Un4QgQ==", "e8bca214-53b8-49c0-a78d-d263fcb28f90" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Description", "DurationInDays", "IsOneTimePayment", "IsUnlimited", "MaxTrips", "Name" },
                values: new object[] { new DateTime(2026, 1, 8, 12, 8, 22, 60, DateTimeKind.Utc).AddTicks(1767), "5 free trips for 30 days", 30, false, false, 5, "Free Trial" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "DurationInDays", "IsOneTimePayment", "IsUnlimited", "MaxTrips", "Name", "Price" },
                values: new object[] { new DateTime(2026, 1, 8, 12, 8, 22, 60, DateTimeKind.Utc).AddTicks(1775), "20 trips per month", 30, false, false, 20, "Monthly Pro", 30m });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "DurationInDays", "IsOneTimePayment", "IsUnlimited", "MaxTrips", "Name" },
                values: new object[] { new DateTime(2026, 1, 8, 12, 8, 22, 60, DateTimeKind.Utc).AddTicks(1781), "Unlimited trips for one year", 365, true, true, null, "Yearly Unlimited" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "DurationInDays", "IsOneTimePayment", "IsUnlimited", "MaxTrips", "Name" },
                values: new object[] { new DateTime(2026, 1, 8, 12, 8, 22, 60, DateTimeKind.Utc).AddTicks(1783), "5 free orders for 30 days", 30, false, false, 5, "Free Trial" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOneTimePayment",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "IsUnlimited",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "MaxTrips",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "CourierSubscription");

            migrationBuilder.DropColumn(
                name: "UsedTrips",
                table: "CourierSubscription");

            migrationBuilder.AddColumn<int>(
                name: "MaxOrders",
                table: "Subscription",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                columns: new[] { "CreatedAt", "Description", "DurationInDays", "MaxOrders", "Name" },
                values: new object[] { new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2564), "Basic package for couriers with 5 allowed orders", 0, 5, "Basic" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "DurationInDays", "MaxOrders", "Name", "Price" },
                values: new object[] { new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2569), "Standard package for couriers with 10 allowed orders", 0, 10, "Standard", 50m });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "DurationInDays", "MaxOrders", "Name" },
                values: new object[] { new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2574), "Premium package for couriers with 20 allowed orders", 0, 20, "Premium" });

            migrationBuilder.UpdateData(
                table: "Subscription",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Description", "DurationInDays", "MaxOrders", "Name" },
                values: new object[] { new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2576), "Basic package for suppliers with 5 allowed orders", 0, 5, "Basic" });

            migrationBuilder.InsertData(
                table: "Subscription",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationInDays", "MaxOrders", "Name", "Price", "UserType" },
                values: new object[,]
                {
                    { 5, new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2579), "Standard package for suppliers with 10 allowed orders", 0, 10, "Standard", 50m, "Supplier" },
                    { 6, new DateTime(2025, 12, 29, 4, 36, 56, 12, DateTimeKind.Utc).AddTicks(2581), "Premium package for suppliers with 20 allowed orders", 0, 20, "Premium", 100m, "Supplier" }
                });
        }
    }
}
