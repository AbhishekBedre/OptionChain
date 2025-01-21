using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class RFactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "StockData",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RFactors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DayHigh = table.Column<double>(type: "float", nullable: false),
                    DayLow = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RFactor = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFactors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockDataIndex_Symbol_EntryDate_Time",
                table: "StockData",
                columns: new[] { "Symbol", "EntryDate", "Time" });

            migrationBuilder.CreateIndex(
                name: "IX_RFactorIndex_Symbol_EntryDate_Time",
                table: "RFactors",
                columns: new[] { "Symbol", "EntryDate", "Time" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RFactors");

            migrationBuilder.DropIndex(
                name: "IX_StockDataIndex_Symbol_EntryDate_Time",
                table: "StockData");

            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "StockData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
