using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class _1111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Packages");

            migrationBuilder.AddColumn<double>(
                name: "Puck_up_lang",
                table: "Requests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Puck_up_lat",
                table: "Requests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Lang",
                table: "Packages",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "Packages",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShipmentReviewID",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShipmentReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipmentReview_Packages_PackageID",
                        column: x => x.PackageID,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c226ddbb-ad1f-4ff3-9b9b-7de523f36add", "AQAAAAIAAYagAAAAECwDj4eoS12EIZoLNb9Cp9mat0LoulHoZ/VfJIMMH55ncFkwQPOt2ROIcyd9UHegKg==", "380b1fb1-01ce-4c87-9bd4-d670de5991c2" });

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentReview_PackageID",
                table: "ShipmentReview",
                column: "PackageID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipmentReview");

            migrationBuilder.DropColumn(
                name: "Puck_up_lang",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Puck_up_lat",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Lang",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ShipmentReviewID",
                table: "Packages");

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Packages",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e908b58f-7a40-4a32-81ba-cb9e62b2607d", "AQAAAAIAAYagAAAAEBxsAmTFz/R/607KXGZr/IM1AcKzQ0nC1msWbLnAG50aKk2OygtUl8pdvqHiqx882g==", "2ccaca21-4e1c-45a1-b7d6-51a3a00f5caf" });
        }
    }
}
