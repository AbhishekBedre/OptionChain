using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations.UpStoxDb
{
    public partial class PreComputerTableAndIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreComputedDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DaysHigh = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DaysLow = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DaysAverageClose = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DaysAverageVolume = table.Column<long>(type: "bigint", nullable: false),
                    DaysATR = table.Column<long>(type: "bigint", nullable: false),
                    DaysMedianATR = table.Column<long>(type: "bigint", nullable: false),
                    DaysTrendScore = table.Column<int>(type: "int", nullable: false),
                    DaysVWAP = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DaysStdDevClose = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DaysStdDevVolume = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DaysGreenPercentage = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DaysAboveVWAPPercentage = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DaysHighLowRangePercentage = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DaysAverageBodySize = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreComputedDatas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OHLCs_CreatedDate_StockMetaDataId",
                table: "OHLCs",
                columns: new[] { "CreatedDate", "StockMetaDataId" });

            migrationBuilder.CreateIndex(
                name: "IX_OHLCs_CreatedDate_Time",
                table: "OHLCs",
                columns: new[] { "CreatedDate", "Time" });

            migrationBuilder.CreateIndex(
                name: "IX_OHLCs_CreatedDate_Time_StockMetaDataId",
                table: "OHLCs",
                columns: new[] { "CreatedDate", "Time", "StockMetaDataId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreComputedDatas");

            migrationBuilder.DropIndex(
                name: "IX_OHLCs_CreatedDate_StockMetaDataId",
                table: "OHLCs");

            migrationBuilder.DropIndex(
                name: "IX_OHLCs_CreatedDate_Time",
                table: "OHLCs");

            migrationBuilder.DropIndex(
                name: "IX_OHLCs_CreatedDate_Time_StockMetaDataId",
                table: "OHLCs");
        }
    }
}
