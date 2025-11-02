using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations.UpStoxDb
{
    public partial class PreComputerTablePreColUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PreviousDayClose",
                table: "PreComputedDatas",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousDayHigh",
                table: "PreComputedDatas",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousDayLow",
                table: "PreComputedDatas",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousDayClose",
                table: "PreComputedDatas");

            migrationBuilder.DropColumn(
                name: "PreviousDayHigh",
                table: "PreComputedDatas");

            migrationBuilder.DropColumn(
                name: "PreviousDayLow",
                table: "PreComputedDatas");
        }
    }
}
