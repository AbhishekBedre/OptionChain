using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class SameOpenLowHigh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SameOpenLowHigh",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PChange = table.Column<double>(type: "float", nullable: true),
                    LastPrice = table.Column<double>(type: "float", nullable: true),
                    Change = table.Column<double>(type: "float", nullable: true),
                    DayHigh = table.Column<double>(type: "float", nullable: true),
                    DayLow = table.Column<double>(type: "float", nullable: true),
                    TFactor = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SameOpenLowHigh");
        }
    }
}
