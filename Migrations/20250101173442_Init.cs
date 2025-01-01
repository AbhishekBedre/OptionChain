using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllOptionData",
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
                    UnderlyingValue = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllOptionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrentExpiryOptionDaata",
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
                    UnderlyingValue = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentExpiryOptionDaata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Summary",
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
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summary", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllOptionData");

            migrationBuilder.DropTable(
                name: "CurrentExpiryOptionDaata");

            migrationBuilder.DropTable(
                name: "Summary");
        }
    }
}
