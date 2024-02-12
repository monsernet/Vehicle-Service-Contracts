using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSP.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceContractManualParts1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScAdditionalParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartQty = table.Column<int>(type: "int", nullable: false),
                    PartUnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceContractId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScAdditionalParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScAdditionalParts_ServiceContracts_ServiceContractId",
                        column: x => x.ServiceContractId,
                        principalTable: "ServiceContracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScAdditionalParts_ServiceContractId",
                table: "ScAdditionalParts",
                column: "ServiceContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScAdditionalParts");
        }
    }
}
