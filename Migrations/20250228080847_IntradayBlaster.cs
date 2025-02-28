using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class IntradayBlaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "SameOpenLowHigh",
                newName: "SectorStocksResponse");

            migrationBuilder.AddColumn<double>(
                name: "Open",
                table: "SectorStocksResponse",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "SectorStocksResponse",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FiiDiiActivitys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BuyValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SellValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiiDiiActivitys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntradayBlasts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastPrice = table.Column<double>(type: "float", nullable: false),
                    PrevLastPrice = table.Column<double>(type: "float", nullable: false),
                    PChange = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Counter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntradayBlasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklySectorUpdate",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WeekEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WeeklyAverage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PChange = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntradayBlastIndex_Symbol_EntryDate_Time",
                table: "IntradayBlasts",
                columns: new[] { "Symbol", "EntryDate", "Time" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FiiDiiActivitys");

            migrationBuilder.DropTable(
                name: "IntradayBlasts");

            migrationBuilder.DropTable(
                name: "WeeklySectorUpdate");

            migrationBuilder.DropColumn(
                name: "Open",
                table: "SectorStocksResponse");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "SectorStocksResponse");

            migrationBuilder.RenameTable(
                name: "SectorStocksResponse",
                newName: "SameOpenLowHigh");
        }
    }
}
