using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class BroderMarkets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BroderMarkets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Index = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndexSymbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Last = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Variation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentChange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Open = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousClose = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearHigh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearLow = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IndicativeClose = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PB = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Declines = table.Column<int>(type: "int", nullable: false),
                    Advances = table.Column<int>(type: "int", nullable: false),
                    Unchanged = table.Column<int>(type: "int", nullable: false),
                    PerChange365d = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date365dAgo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Chart365dPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date30dAgo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PerChange30d = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Chart30dPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousDay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OneWeekAgo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OneMonthAgo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OneYearAgo = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BroderMarkets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BroderMarkets");
        }
    }
}
