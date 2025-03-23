using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class IndexOnDailyStockTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "DailyStockData",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_DailyStockDataIndex_EntryDate",
                table: "DailyStockData",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_DailyStockDataIndex_Symbol",
                table: "DailyStockData",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DailyStockDataIndex_EntryDate",
                table: "DailyStockData");

            migrationBuilder.DropIndex(
                name: "IX_DailyStockDataIndex_Symbol",
                table: "DailyStockData");

            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "DailyStockData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
