using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    /// <inheritdoc />
    public partial class FKUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockData_StockMetaData_MetaId",
                table: "StockData");

            migrationBuilder.DropIndex(
                name: "IX_StockData_MetaId",
                table: "StockData");

            migrationBuilder.DropColumn(
                name: "MetaId",
                table: "StockData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MetaId",
                table: "StockData",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockData_MetaId",
                table: "StockData",
                column: "MetaId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockData_StockMetaData_MetaId",
                table: "StockData",
                column: "MetaId",
                principalTable: "StockMetaData",
                principalColumn: "Id");
        }
    }
}
