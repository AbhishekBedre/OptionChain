using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptionChain.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advance",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Declines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Advances = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unchanged = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllOptionData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AskPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AskQty = table.Column<int>(type: "int", nullable: false),
                    BidPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BidQty = table.Column<int>(type: "int", nullable: false),
                    Change = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangeInOpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpliedVolatility = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PChange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PChangeInOpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StrikePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBuyQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalSellQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalTradedVolume = table.Column<int>(type: "int", nullable: false),
                    Underlying = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderlyingValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllOptionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankExpiryOptionData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AskPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AskQty = table.Column<int>(type: "int", nullable: false),
                    BidPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BidQty = table.Column<int>(type: "int", nullable: false),
                    Change = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangeInOpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpliedVolatility = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PChange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PChangeInOpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StrikePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBuyQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalSellQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalTradedVolume = table.Column<int>(type: "int", nullable: false),
                    Underlying = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderlyingValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    AskPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AskQty = table.Column<int>(type: "int", nullable: false),
                    BidPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BidQty = table.Column<int>(type: "int", nullable: false),
                    Change = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangeInOpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpliedVolatility = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PChange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PChangeInOpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StrikePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBuyQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalSellQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalTradedVolume = table.Column<int>(type: "int", nullable: false),
                    Underlying = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderlyingValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "BroderMarkets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Index = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndexSymbol = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Last = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Variation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentChange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Open = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousClose = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearHigh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearLow = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IndicativeClose = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PB = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Declines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Advances = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unchanged = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date365dAgo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chart365dPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date30dAgo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerChange30d = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Chart30dPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousDay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OneWeekAgo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OneMonthAgo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OneYearAgo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BroderMarkets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrentExpiryOptionDaata",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AskPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AskQty = table.Column<int>(type: "int", nullable: false),
                    BidPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BidQty = table.Column<int>(type: "int", nullable: false),
                    Change = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangeInOpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpliedVolatility = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PChange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PChangeInOpenInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StrikePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBuyQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalSellQuantity = table.Column<int>(type: "int", nullable: false),
                    TotalTradedVolume = table.Column<int>(type: "int", nullable: false),
                    Underlying = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnderlyingValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentExpiryOptionDaata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyStocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Series = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Open = table.Column<double>(type: "float", nullable: false),
                    DayHigh = table.Column<double>(type: "float", nullable: false),
                    DayLow = table.Column<double>(type: "float", nullable: false),
                    LastPrice = table.Column<double>(type: "float", nullable: false),
                    PreviousClose = table.Column<double>(type: "float", nullable: false),
                    Change = table.Column<double>(type: "float", nullable: false),
                    PChange = table.Column<double>(type: "float", nullable: false),
                    TotalTradedVolume = table.Column<long>(type: "bigint", nullable: false),
                    StockIndClosePrice = table.Column<double>(type: "float", nullable: false),
                    TotalTradedValue = table.Column<double>(type: "float", nullable: false),
                    LastUpdateTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearHigh = table.Column<double>(type: "float", nullable: false),
                    Ffmc = table.Column<double>(type: "float", nullable: false),
                    YearLow = table.Column<double>(type: "float", nullable: false),
                    NearWKH = table.Column<double>(type: "float", nullable: false),
                    NearWKL = table.Column<double>(type: "float", nullable: false),
                    Date365dAgo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chart365dPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date30dAgo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chart30dPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChartTodayPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyStocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FiiDiiActivitys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BuyValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SellValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiiDiiActivitys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntradayBlasts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastPrice = table.Column<double>(type: "float", nullable: false),
                    PrevLastPrice = table.Column<double>(type: "float", nullable: false),
                    PChange = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Counter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntradayBlasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RFactors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DayHigh = table.Column<double>(type: "float", nullable: false),
                    DayLow = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RFactor = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFactors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MappingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SectorStocksResponse",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PChange = table.Column<double>(type: "float", nullable: true),
                    LastPrice = table.Column<double>(type: "float", nullable: true),
                    Open = table.Column<double>(type: "float", nullable: true),
                    Change = table.Column<double>(type: "float", nullable: true),
                    DayHigh = table.Column<double>(type: "float", nullable: true),
                    DayLow = table.Column<double>(type: "float", nullable: true),
                    TFactor = table.Column<double>(type: "float", nullable: true),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNifty50 = table.Column<int>(type: "int", nullable: false),
                    IsNifty100 = table.Column<int>(type: "int", nullable: false),
                    IsNifty200 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cookie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Series = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Open = table.Column<double>(type: "float", nullable: false),
                    DayHigh = table.Column<double>(type: "float", nullable: false),
                    DayLow = table.Column<double>(type: "float", nullable: false),
                    LastPrice = table.Column<double>(type: "float", nullable: false),
                    PreviousClose = table.Column<double>(type: "float", nullable: false),
                    Change = table.Column<double>(type: "float", nullable: false),
                    PChange = table.Column<double>(type: "float", nullable: false),
                    TotalTradedVolume = table.Column<long>(type: "bigint", nullable: false),
                    StockIndClosePrice = table.Column<double>(type: "float", nullable: false),
                    TotalTradedValue = table.Column<double>(type: "float", nullable: false),
                    LastUpdateTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearHigh = table.Column<double>(type: "float", nullable: false),
                    Ffmc = table.Column<double>(type: "float", nullable: false),
                    YearLow = table.Column<double>(type: "float", nullable: false),
                    NearWKH = table.Column<double>(type: "float", nullable: false),
                    NearWKL = table.Column<double>(type: "float", nullable: false),
                    Date365dAgo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chart365dPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date30dAgo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chart30dPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChartTodayPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockMetaData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFNOSec = table.Column<bool>(type: "bit", nullable: false),
                    IsCASec = table.Column<bool>(type: "bit", nullable: false),
                    IsSLBSec = table.Column<bool>(type: "bit", nullable: false),
                    IsDebtSec = table.Column<bool>(type: "bit", nullable: false),
                    IsSuspended = table.Column<bool>(type: "bit", nullable: false),
                    IsETFSec = table.Column<bool>(type: "bit", nullable: false),
                    IsDelisted = table.Column<bool>(type: "bit", nullable: false),
                    Isin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlbIsin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMunicipalBond = table.Column<bool>(type: "bit", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsNifty50 = table.Column<bool>(type: "bit", nullable: false),
                    IsNifty100 = table.Column<bool>(type: "bit", nullable: false),
                    IsNifty200 = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMetaData", x => x.Id);
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
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImgeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedEmail = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklySectorUpdate",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WeekEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WeeklyAverage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PChange = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_OptionDataIndex_EntryDate",
                table: "AllOptionData",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_BankExpiryOptionDataIndex_EntryDate",
                table: "BankExpiryOptionData",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_BankOptionDataIndex_EntryDate",
                table: "BankOptionData",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_BroderMarketsIndex_EntryDate",
                table: "BroderMarkets",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_BroderMarketsIndex_Symbol_EntryDate_Key",
                table: "BroderMarkets",
                columns: new[] { "Key", "IndexSymbol", "EntryDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BroderMarketsIndex_Symbol_EntryDate_Time_Key",
                table: "BroderMarkets",
                columns: new[] { "Key", "IndexSymbol", "EntryDate", "Time" });

            migrationBuilder.CreateIndex(
                name: "IX_FilteredOptionDataIndex_EntryDate",
                table: "CurrentExpiryOptionDaata",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_DailyStockIndex_EntryDate",
                table: "DailyStocks",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_DailyStockIndex_Symbol_EntryDate_Time",
                table: "DailyStocks",
                columns: new[] { "Symbol", "EntryDate", "Time" });

            migrationBuilder.CreateIndex(
                name: "IX_IntradayBlastIndex_Symbol_EntryDate_Time",
                table: "IntradayBlasts",
                columns: new[] { "Symbol", "EntryDate", "Time" });

            migrationBuilder.CreateIndex(
                name: "IX_RFactorIndex_Symbol_EntryDate_Time",
                table: "RFactors",
                columns: new[] { "Symbol", "EntryDate", "Time" });

            migrationBuilder.CreateIndex(
                name: "IX_StockDataIndex_EntryDate",
                table: "StockData",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_StockDataIndex_Symbol_EntryDate_Time",
                table: "StockData",
                columns: new[] { "Symbol", "EntryDate", "Time" });

            migrationBuilder.CreateIndex(
                name: "IX_StockMetaDataIndex_EntryDate",
                table: "StockMetaData",
                column: "EntryDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advance");

            migrationBuilder.DropTable(
                name: "AllOptionData");

            migrationBuilder.DropTable(
                name: "BankExpiryOptionData");

            migrationBuilder.DropTable(
                name: "BankOptionData");

            migrationBuilder.DropTable(
                name: "BankSummary");

            migrationBuilder.DropTable(
                name: "BroderMarkets");

            migrationBuilder.DropTable(
                name: "CurrentExpiryOptionDaata");

            migrationBuilder.DropTable(
                name: "DailyStocks");

            migrationBuilder.DropTable(
                name: "FiiDiiActivitys");

            migrationBuilder.DropTable(
                name: "IntradayBlasts");

            migrationBuilder.DropTable(
                name: "RFactors");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "SectorStocksResponse");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "StockData");

            migrationBuilder.DropTable(
                name: "StockMetaData");

            migrationBuilder.DropTable(
                name: "Summary");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WeeklySectorUpdate");
        }
    }
}
