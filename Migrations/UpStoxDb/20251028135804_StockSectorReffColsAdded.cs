using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations.UpStoxDb
{
    public partial class StockSectorReffColsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "StockMetaDataId",
                table: "SectorStockMetaDatas",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "SectorDisplayName",
                table: "SectorStockMetaDatas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "SectorId",
                table: "SectorStockMetaDatas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectorDisplayName",
                table: "SectorStockMetaDatas");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "SectorStockMetaDatas");

            migrationBuilder.AlterColumn<string>(
                name: "StockMetaDataId",
                table: "SectorStockMetaDatas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
