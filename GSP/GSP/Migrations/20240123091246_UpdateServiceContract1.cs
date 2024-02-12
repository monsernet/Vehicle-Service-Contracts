using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServiceContract1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ServiceContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ServiceContracts");
        }
    }
}
