using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class addddddddddd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Puck_up_lat",
                table: "Requests",
                newName: "PickupLng");

            migrationBuilder.RenameColumn(
                name: "Puck_up_lang",
                table: "Requests",
                newName: "PickupLat");

            migrationBuilder.AlterColumn<string>(
                name: "VehcelLicensePhotoFront",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VehcelLicensePhotoBack",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LicensePhotoFront",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LicensePhotoBack",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0404787-7581-4ebe-ab22-0d0005a8691f", "AQAAAAIAAYagAAAAECt2kMFMjmJq01bR4ghdcr6HOEzj4COrH1ceNb/U8Q6VcED7DA3Fbk2lAE0HisHHNA==", "d0d7e8ee-ed54-4f8c-8423-d68bce1ac5d0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PickupLng",
                table: "Requests",
                newName: "Puck_up_lat");

            migrationBuilder.RenameColumn(
                name: "PickupLat",
                table: "Requests",
                newName: "Puck_up_lang");

            migrationBuilder.AlterColumn<string>(
                name: "VehcelLicensePhotoFront",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehcelLicensePhotoBack",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicensePhotoFront",
                table: "Courier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicensePhotoBack",
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
                values: new object[] { "ef413cc2-3187-4e66-987f-e09b00b24f5d", "AQAAAAIAAYagAAAAEMPRecauRFb9cfhEVgtllObMmEnUyvbWGiFCcWtErKz8051lEL0Zgv7HJIgJCZQ2/Q==", "a38b51ef-8c55-410f-89b2-a244a7e7d4b7" });
        }
    }
}
