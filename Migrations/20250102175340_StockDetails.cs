using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    /// <inheritdoc />
    public partial class StockDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advance",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Declines = table.Column<int>(type: "int", nullable: false),
                    Advances = table.Column<int>(type: "int", nullable: false),
                    Unchanged = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockMetaData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFNOSec = table.Column<bool>(type: "bit", nullable: false),
                    IsCASec = table.Column<bool>(type: "bit", nullable: false),
                    IsSLBSec = table.Column<bool>(type: "bit", nullable: false),
                    IsDebtSec = table.Column<bool>(type: "bit", nullable: false),
                    IsSuspended = table.Column<bool>(type: "bit", nullable: false),
                    IsETFSec = table.Column<bool>(type: "bit", nullable: false),
                    IsDelisted = table.Column<bool>(type: "bit", nullable: false),
                    Isin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SlbIsin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMunicipalBond = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMetaData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Series = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Open = table.Column<double>(type: "float", nullable: false),
                    DayHigh = table.Column<double>(type: "float", nullable: false),
                    DayLow = table.Column<double>(type: "float", nullable: false),
                    LastPrice = table.Column<double>(type: "float", nullable: false),
                    PreviousClose = table.Column<double>(type: "float", nullable: false),
                    Change = table.Column<double>(type: "float", nullable: false),
                    PChange = table.Column<double>(type: "float", nullable: false),
                    TotalTradedVolume = table.Column<long>(type: "bigint", nullable: false),
                    StockIndClosePrice = table.Column<double>(type: "float", nullable: false),
                    TotalTradedValue = table.Column<double>(type: "float", nullable: false),
                    LastUpdateTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearHigh = table.Column<double>(type: "float", nullable: false),
                    Ffmc = table.Column<double>(type: "float", nullable: false),
                    YearLow = table.Column<double>(type: "float", nullable: false),
                    NearWKH = table.Column<double>(type: "float", nullable: false),
                    NearWKL = table.Column<double>(type: "float", nullable: false),
                    PerChange365d = table.Column<double>(type: "float", nullable: false),
                    Date365dAgo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chart365dPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date30dAgo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerChange30d = table.Column<double>(type: "float", nullable: false),
                    Chart30dPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChartTodayPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockData_StockMetaData_MetaId",
                        column: x => x.MetaId,
                        principalTable: "StockMetaData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockData_MetaId",
                table: "StockData",
                column: "MetaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advance");

            migrationBuilder.DropTable(
                name: "StockData");

            migrationBuilder.DropTable(
                name: "StockMetaData");
        }
    }
}
