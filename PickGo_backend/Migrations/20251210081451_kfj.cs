using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class kfj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LicensePhotoBack",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LicensePhotoFront",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehcelLicensePhotoBack",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehcelLicensePhotoFront",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ef413cc2-3187-4e66-987f-e09b00b24f5d", "AQAAAAIAAYagAAAAEMPRecauRFb9cfhEVgtllObMmEnUyvbWGiFCcWtErKz8051lEL0Zgv7HJIgJCZQ2/Q==", "a38b51ef-8c55-410f-89b2-a244a7e7d4b7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicensePhotoBack",
                table: "Courier");

            migrationBuilder.DropColumn(
                name: "LicensePhotoFront",
                table: "Courier");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Courier");

            migrationBuilder.DropColumn(
                name: "VehcelLicensePhotoBack",
                table: "Courier");

            migrationBuilder.DropColumn(
                name: "VehcelLicensePhotoFront",
                table: "Courier");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c226ddbb-ad1f-4ff3-9b9b-7de523f36add", "AQAAAAIAAYagAAAAECwDj4eoS12EIZoLNb9Cp9mat0LoulHoZ/VfJIMMH55ncFkwQPOt2ROIcyd9UHegKg==", "380b1fb1-01ce-4c87-9bd4-d670de5991c2" });
        }
    }
}
