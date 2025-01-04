using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class RemovePer30d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerChange30d",
                table: "StockData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PerChange30d",
                table: "StockData",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
