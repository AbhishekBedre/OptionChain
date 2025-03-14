using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class RemoveOptionCols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AskPrice",
                table: "AllOptionData");

            migrationBuilder.DropColumn(
                name: "AskQty",
                table: "AllOptionData");

            migrationBuilder.DropColumn(
                name: "BidPrice",
                table: "AllOptionData");

            migrationBuilder.DropColumn(
                name: "BidQty",
                table: "AllOptionData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AskPrice",
                table: "AllOptionData",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "AskQty",
                table: "AllOptionData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "BidPrice",
                table: "AllOptionData",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BidQty",
                table: "AllOptionData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
