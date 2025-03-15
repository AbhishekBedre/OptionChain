using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class AddedIndexOnTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StockDataIndex_Symbol",
                table: "StockData",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_RFactorIndex_Symbol",
                table: "RFactors",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockDataIndex_Symbol",
                table: "StockData");

            migrationBuilder.DropIndex(
                name: "IX_RFactorIndex_Symbol",
                table: "RFactors");
        }
    }
}
