using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSP.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscToServiceContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AddPartsDiscount",
                table: "ServiceContracts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PartsDisc",
                table: "ServiceContracts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ServDisc",
                table: "ServiceContracts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "ServiceContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "ServiceContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddPartsDiscount",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "PartsDisc",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "ServDisc",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "ServiceContracts");

            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "ServiceContracts");
        }
    }
}
