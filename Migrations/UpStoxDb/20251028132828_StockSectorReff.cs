using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations.UpStoxDb
{
    public partial class StockSectorReff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time",
                table: "OHLCs",
                type: "time",
                nullable: true,
                defaultValueSql: "CONVERT(VARCHAR(5), GETUTCDATE(), 108)",
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true,
                oldDefaultValueSql: "CONVERT(VARCHAR(5), GETDATE(), 108)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OHLCs",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "MarketMetaDatas",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "AuthDetails",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.CreateTable(
                name: "SectorStockMetaDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockMetaDataId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorStockMetaDatas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SectorStockMetaDatas");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time",
                table: "OHLCs",
                type: "time",
                nullable: true,
                defaultValueSql: "CONVERT(VARCHAR(5), GETDATE(), 108)",
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true,
                oldDefaultValueSql: "CONVERT(VARCHAR(5), GETUTCDATE(), 108)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OHLCs",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "MarketMetaDatas",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "AuthDetails",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
