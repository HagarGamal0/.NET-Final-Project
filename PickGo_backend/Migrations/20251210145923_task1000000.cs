using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class task1000000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ba824b45-c2c9-4327-b7bb-ac2d27388fdd", "AQAAAAIAAYagAAAAEMOXqsSchOeZc05df96BItZDLthbp20jbjbpG8z8oevhmcpACrCdm+TmdMYajUz+/g==", "71494f6f-8c43-47e0-ad92-7ed09fcf6823" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0404787-7581-4ebe-ab22-0d0005a8691f", "AQAAAAIAAYagAAAAECt2kMFMjmJq01bR4ghdcr6HOEzj4COrH1ceNb/U8Q6VcED7DA3Fbk2lAE0HisHHNA==", "d0d7e8ee-ed54-4f8c-8423-d68bce1ac5d0" });
        }
    }
}
