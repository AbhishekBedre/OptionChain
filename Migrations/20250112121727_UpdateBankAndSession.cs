using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class UpdateBankAndSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankExpiryOptionData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AskPrice = table.Column<double>(type: "float", nullable: false),
                    AskQty = table.Column<int>(type: "int", nullable: false),
                    BidPrice = table.Column<double>(type: "float", nullable: false),
                    BidQty = table.Column<int>(type: "int", nullable: false),
                    Change = table.Column<double>(type: "float", nullable: false),
                    ChangeInOpenInterest = table.Column<double>(type: "float", nullable: false),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpliedVolatility = table.Column<double>(type: "float", nullable: false),
                    LastPrice = table.Column<double>(type: "float", nullable: false),
                    OpenInterest = table.Column<double>(type: "float", nullable: false),
                    PChange = table.Column<double>(type: "float", nullable: false),
                    PChangeInOpenInterest = table.Column<double>(type: "float", nullable: false),
                    StrikePrice = table.Column<double>(type: "float", nullable: false),
                    TotalBuyQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalSellQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalTradedVolume = table.Column<int>(type: "int", nullable: false),
                    Underlying = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderlyingValue = table.Column<double>(type: "float", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankExpiryOptionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankOptionData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AskPrice = table.Column<double>(type: "float", nullable: false),
                    AskQty = table.Column<int>(type: "int", nullable: false),
                    BidPrice = table.Column<double>(type: "float", nullable: false),
                    BidQty = table.Column<int>(type: "int", nullable: false),
                    Change = table.Column<double>(type: "float", nullable: false),
                    ChangeInOpenInterest = table.Column<double>(type: "float", nullable: false),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpliedVolatility = table.Column<double>(type: "float", nullable: false),
                    LastPrice = table.Column<double>(type: "float", nullable: false),
                    OpenInterest = table.Column<double>(type: "float", nullable: false),
                    PChange = table.Column<double>(type: "float", nullable: false),
                    PChangeInOpenInterest = table.Column<double>(type: "float", nullable: false),
                    StrikePrice = table.Column<double>(type: "float", nullable: false),
                    TotalBuyQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalSellQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalTradedVolume = table.Column<int>(type: "int", nullable: false),
                    Underlying = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderlyingValue = table.Column<double>(type: "float", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankOptionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankSummary",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotOICE = table.Column<double>(type: "float", nullable: false),
                    TotOIPE = table.Column<double>(type: "float", nullable: false),
                    TotVolCE = table.Column<double>(type: "float", nullable: false),
                    TotVolPE = table.Column<double>(type: "float", nullable: false),
                    CEPEOIDiff = table.Column<double>(type: "float", nullable: false),
                    CEPEVolDiff = table.Column<double>(type: "float", nullable: false),
                    CEPEOIPrevDiff = table.Column<double>(type: "float", nullable: false),
                    CEPEVolPrevDiff = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankSummary", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankExpiryOptionData");

            migrationBuilder.DropTable(
                name: "BankOptionData");

            migrationBuilder.DropTable(
                name: "BankSummary");
        }
    }
}
