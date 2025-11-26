using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class jjjjjjj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxOrders = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourierSubscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourierSubscription_Courier_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Courier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourierSubscription_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierSubscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierSubscription_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierSubscription_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Subscription",
                columns: new[] { "Id", "Description", "DurationInDays", "MaxOrders", "Name", "Price", "UserType" },
                values: new object[,]
                {
                    { 1, "Basic package for couriers with 5 allowed orders", 0, 5, "Basic", 0m, "Courier" },
                    { 2, "Standard package for couriers with 10 allowed orders", 0, 10, "Standard", 50m, "Courier" },
                    { 3, "Premium package for couriers with 20 allowed orders", 0, 20, "Premium", 100m, "Courier" },
                    { 4, "Basic package for suppliers with 5 allowed orders", 0, 5, "Basic", 0m, "Supplier" },
                    { 5, "Standard package for suppliers with 10 allowed orders", 0, 10, "Standard", 50m, "Supplier" },
                    { 6, "Premium package for suppliers with 20 allowed orders", 0, 20, "Premium", 100m, "Supplier" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourierSubscription_CourierId",
                table: "CourierSubscription",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierSubscription_SubscriptionId",
                table: "CourierSubscription",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierSubscription_SubscriptionId",
                table: "SupplierSubscription",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierSubscription_SupplierId",
                table: "SupplierSubscription",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourierSubscription");

            migrationBuilder.DropTable(
                name: "SupplierSubscription");

            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
