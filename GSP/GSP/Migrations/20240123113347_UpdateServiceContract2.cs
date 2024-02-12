using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServiceContract2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "ServiceContracts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "ServiceContracts");
        }
    }
}
