using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class BroderMarketIndexAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsNifty100",
                table: "SectorStocksResponse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsNifty200",
                table: "SectorStocksResponse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsNifty50",
                table: "SectorStocksResponse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "BroderMarkets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IndexSymbol",
                table: "BroderMarkets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BroderMarketsIndex_Symbol_EntryDate_Key",
                table: "BroderMarkets",
                columns: new[] { "Key", "IndexSymbol", "EntryDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BroderMarketsIndex_Symbol_EntryDate_Time_Key",
                table: "BroderMarkets",
                columns: new[] { "Key", "IndexSymbol", "EntryDate", "Time" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BroderMarketsIndex_Symbol_EntryDate_Key",
                table: "BroderMarkets");

            migrationBuilder.DropIndex(
                name: "IX_BroderMarketsIndex_Symbol_EntryDate_Time_Key",
                table: "BroderMarkets");

            migrationBuilder.DropColumn(
                name: "IsNifty100",
                table: "SectorStocksResponse");

            migrationBuilder.DropColumn(
                name: "IsNifty200",
                table: "SectorStocksResponse");

            migrationBuilder.DropColumn(
                name: "IsNifty50",
                table: "SectorStocksResponse");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "BroderMarkets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IndexSymbol",
                table: "BroderMarkets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
