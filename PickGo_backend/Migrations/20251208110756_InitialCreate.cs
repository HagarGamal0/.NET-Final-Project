using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickGo_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customers_AspNetUsers_UserId",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Requests_RequestID",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Supplier_SupplierId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_customers_UserId",
                table: "customers");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "Requests",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "RequestID",
                table: "Packages",
                newName: "RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_RequestID",
                table: "Packages",
                newName: "IX_Packages_RequestId");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "CODAmount",
                table: "Requests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DropoffAddress",
                table: "Requests",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DropoffLat",
                table: "Requests",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DropoffLng",
                table: "Requests",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemsDescription",
                table: "Requests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Requests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PickupAddress",
                table: "Requests",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "PickupLat",
                table: "Requests",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PickupLng",
                table: "Requests",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                table: "Requests",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverPhone",
                table: "Requests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Requests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "customers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c9b07bb-3cd1-4163-bc6a-89bcf41fc901", "AQAAAAIAAYagAAAAEDrbSFFHku7klWSgLj1e9EjIbimonEagL6EhxjVQYbh2BuULfdbqRXBmI8PHGhUMhw==", "925c3d88-accc-4296-968c-6e5eff61a10c" });

            migrationBuilder.CreateIndex(
                name: "IX_customers_UserId",
                table: "customers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_customers_AspNetUsers_UserId",
                table: "customers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Requests_RequestId",
                table: "Packages",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Supplier_SupplierId",
                table: "Requests",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customers_AspNetUsers_UserId",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Requests_RequestId",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Supplier_SupplierId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_customers_UserId",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "CODAmount",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DropoffAddress",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DropoffLat",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DropoffLng",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ItemsDescription",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PickupAddress",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PickupLat",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PickupLng",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ReceiverName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ReceiverPhone",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "customers");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Requests",
                newName: "Source");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "Packages",
                newName: "RequestID");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_RequestId",
                table: "Packages",
                newName: "IX_Packages_RequestID");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "customers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5dca1eb3-0fc5-455c-a495-298953cf9a5d", "AQAAAAIAAYagAAAAEIxY71Qo9fDa9nwaMed3Pl+BU7CNuaaFyNIFxsrbk5791FpvuICXmrxjVKGBuTIyww==", "4066f232-209c-4e82-a711-a81cece973c1" });

            migrationBuilder.CreateIndex(
                name: "IX_customers_UserId",
                table: "customers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_customers_AspNetUsers_UserId",
                table: "customers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Requests_RequestID",
                table: "Packages",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Supplier_SupplierId",
                table: "Requests",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
