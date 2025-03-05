using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class OptionDataColUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnderlyingValue",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "StrikePrice",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "PChangeInOpenInterest",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "PChange",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "OpenInterest",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "LastPrice",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ImpliedVolatility",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChangeInOpenInterest",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Change",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "BidPrice",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "AskPrice",
                table: "CurrentExpiryOptionDaata",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnderlyingValue",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "StrikePrice",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "PChangeInOpenInterest",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "PChange",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "OpenInterest",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "LastPrice",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ImpliedVolatility",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChangeInOpenInterest",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Change",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "BidPrice",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "AskPrice",
                table: "AllOptionData",
                type: "decimal(18,16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "UnderlyingValue",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "StrikePrice",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "PChangeInOpenInterest",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "PChange",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "OpenInterest",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "LastPrice",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "ImpliedVolatility",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "ChangeInOpenInterest",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "Change",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "BidPrice",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "AskPrice",
                table: "CurrentExpiryOptionDaata",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "UnderlyingValue",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "StrikePrice",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "PChangeInOpenInterest",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "PChange",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "OpenInterest",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "LastPrice",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "ImpliedVolatility",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "ChangeInOpenInterest",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "Change",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "BidPrice",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");

            migrationBuilder.AlterColumn<double>(
                name: "AskPrice",
                table: "AllOptionData",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,16)");
        }
    }
}
