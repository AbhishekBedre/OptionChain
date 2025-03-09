using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class UpdateStockMetaDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNifty100",
                table: "StockMetaData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNifty200",
                table: "StockMetaData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNifty50",
                table: "StockMetaData",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNifty100",
                table: "StockMetaData");

            migrationBuilder.DropColumn(
                name: "IsNifty200",
                table: "StockMetaData");

            migrationBuilder.DropColumn(
                name: "IsNifty50",
                table: "StockMetaData");
        }
    }
}
