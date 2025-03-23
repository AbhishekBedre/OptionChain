using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class AddIndexRFactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResponseSectorsStocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PChange = table.Column<double>(type: "float", nullable: true),
                    LastPrice = table.Column<double>(type: "float", nullable: true),
                    Open = table.Column<double>(type: "float", nullable: true),
                    Change = table.Column<double>(type: "float", nullable: true),
                    DayHigh = table.Column<double>(type: "float", nullable: true),
                    DayLow = table.Column<double>(type: "float", nullable: true),
                    TFactor = table.Column<double>(type: "float", nullable: true),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFNOSec = table.Column<bool>(type: "bit", nullable: false),
                    IsNifty50 = table.Column<bool>(type: "bit", nullable: false),
                    IsNifty100 = table.Column<bool>(type: "bit", nullable: false),
                    IsNifty200 = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_RFactorIndex_EntryDate",
                table: "RFactors",
                column: "EntryDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponseSectorsStocks");

            migrationBuilder.DropIndex(
                name: "IX_RFactorIndex_EntryDate",
                table: "RFactors");
        }
    }
}
