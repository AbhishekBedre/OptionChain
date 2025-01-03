using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    /// <inheritdoc />
    public partial class NewColsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockData_StockMetaData_MetaId",
                table: "StockData");

            migrationBuilder.DropColumn(
                name: "PerChange365d",
                table: "StockData");

            migrationBuilder.AlterColumn<string>(
                name: "SlbIsin",
                table: "StockMetaData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Isin",
                table: "StockMetaData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDate",
                table: "StockMetaData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MetaId",
                table: "StockData",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDate",
                table: "StockData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Unchanged",
                table: "Advance",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Declines",
                table: "Advance",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Advances",
                table: "Advance",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryDate",
                table: "Advance",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockData_StockMetaData_MetaId",
                table: "StockData",
                column: "MetaId",
                principalTable: "StockMetaData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockData_StockMetaData_MetaId",
                table: "StockData");

            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "StockMetaData");

            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "StockData");

            migrationBuilder.DropColumn(
                name: "EntryDate",
                table: "Advance");

            migrationBuilder.AlterColumn<string>(
                name: "SlbIsin",
                table: "StockMetaData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Isin",
                table: "StockMetaData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MetaId",
                table: "StockData",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PerChange365d",
                table: "StockData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "Unchanged",
                table: "Advance",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Declines",
                table: "Advance",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Advances",
                table: "Advance",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StockData_StockMetaData_MetaId",
                table: "StockData",
                column: "MetaId",
                principalTable: "StockMetaData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
