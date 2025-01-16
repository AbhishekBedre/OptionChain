using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class ColsUpdateAndIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerChange365d",
                table: "BroderMarkets");

            migrationBuilder.AlterColumn<string>(
                name: "Unchanged",
                table: "BroderMarkets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Declines",
                table: "BroderMarkets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Date365dAgo",
                table: "BroderMarkets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Date30dAgo",
                table: "BroderMarkets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Advances",
                table: "BroderMarkets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDate",
                table: "BroderMarkets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "BroderMarkets",
                type: "time",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMetaDataIndex_EntryDate",
                table: "StockMetaData",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_StockDataIndex_EntryDate",
                table: "StockData",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_FilteredOptionDataIndex_EntryDate",
                table: "CurrentExpiryOptionDaata",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_BroderMarketsIndex_EntryDate",
                table: "BroderMarkets",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_BankOptionDataIndex_EntryDate",
                table: "BankOptionData",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_BankExpiryOptionDataIndex_EntryDate",
                table: "BankExpiryOptionData",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_OptionDataIndex_EntryDate",
                table: "AllOptionData",
                column: "EntryDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockMetaDataIndex_EntryDate",
                table: "StockMetaData");

            migrationBuilder.DropIndex(
                name: "IX_StockDataIndex_EntryDate",
                table: "StockData");

            migrationBuilder.DropIndex(
                name: "IX_FilteredOptionDataIndex_EntryDate",
                table: "CurrentExpiryOptionDaata");

            migrationBuilder.DropIndex(
                name: "IX_BroderMarketsIndex_EntryDate",
                table: "BroderMarkets");

            migrationBuilder.DropIndex(
                name: "IX_BankOptionDataIndex_EntryDate",
                table: "BankOptionData");

            migrationBuilder.DropIndex(
                name: "IX_BankExpiryOptionDataIndex_EntryDate",
                table: "BankExpiryOptionData");

            migrationBuilder.DropIndex(
                name: "IX_OptionDataIndex_EntryDate",
                table: "AllOptionData");

            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "BroderMarkets");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "BroderMarkets");

            migrationBuilder.AlterColumn<int>(
                name: "Unchanged",
                table: "BroderMarkets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Declines",
                table: "BroderMarkets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date365dAgo",
                table: "BroderMarkets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date30dAgo",
                table: "BroderMarkets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Advances",
                table: "BroderMarkets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PerChange365d",
                table: "BroderMarkets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
