using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSP.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleTypeToParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "GeniumParts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_GeniumParts_VehicleTypeId",
                table: "GeniumParts",
                column: "VehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeniumParts_VehicleTypes_VehicleTypeId",
                table: "GeniumParts",
                column: "VehicleTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeniumParts_VehicleTypes_VehicleTypeId",
                table: "GeniumParts");

            migrationBuilder.DropIndex(
                name: "IX_GeniumParts_VehicleTypeId",
                table: "GeniumParts");

            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "GeniumParts");
        }
    }
}
