﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OptionChain;

#nullable disable

namespace OptionChain.Migrations
{
    [DbContext(typeof(OptionDbContext))]
    [Migration("20250312155017_BroderMarketIndexAdded")]
    partial class BroderMarketIndexAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("OptionChain.Advance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Advances")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Declines")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<string>("Unchanged")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Advance");
                });

            modelBuilder.Entity("OptionChain.BankExpiryOptionData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal>("AskPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("AskQty")
                        .HasColumnType("int");

                    b.Property<decimal>("BidPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("BidQty")
                        .HasColumnType("int");

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ChangeInOpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExpiryDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ImpliedVolatility")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PChange")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PChangeInOpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("StrikePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<int>("TotalBuyQuantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalSellQuantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalTradedVolume")
                        .HasColumnType("int");

                    b.Property<string>("Underlying")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UnderlyingValue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("EntryDate")
                        .HasDatabaseName("IX_BankExpiryOptionDataIndex_EntryDate");

                    b.ToTable("BankExpiryOptionData");
                });

            modelBuilder.Entity("OptionChain.BankOptionData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal>("AskPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("AskQty")
                        .HasColumnType("int");

                    b.Property<decimal>("BidPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("BidQty")
                        .HasColumnType("int");

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ChangeInOpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExpiryDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ImpliedVolatility")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PChange")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PChangeInOpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("StrikePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<int>("TotalBuyQuantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalSellQuantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalTradedVolume")
                        .HasColumnType("int");

                    b.Property<string>("Underlying")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UnderlyingValue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("EntryDate")
                        .HasDatabaseName("IX_BankOptionDataIndex_EntryDate");

                    b.ToTable("BankOptionData");
                });

            modelBuilder.Entity("OptionChain.BankSummary", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<double>("CEPEOIDiff")
                        .HasColumnType("float");

                    b.Property<double>("CEPEOIPrevDiff")
                        .HasColumnType("float");

                    b.Property<double>("CEPEVolDiff")
                        .HasColumnType("float");

                    b.Property<double>("CEPEVolPrevDiff")
                        .HasColumnType("float");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<double>("TotOICE")
                        .HasColumnType("float");

                    b.Property<double>("TotOIPE")
                        .HasColumnType("float");

                    b.Property<double>("TotVolCE")
                        .HasColumnType("float");

                    b.Property<double>("TotVolPE")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("BankSummary");
                });

            modelBuilder.Entity("OptionChain.BroderMarkets", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Advances")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Chart30dPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Chart365dPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DY")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Date30dAgo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date365dAgo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Declines")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("High")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Index")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IndexSymbol")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("IndicativeClose")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Last")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Low")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("OneMonthAgo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("OneWeekAgo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("OneYearAgo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Open")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PB")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PE")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal?>("PerChange30d")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PercentChange")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PreviousClose")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PreviousDay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<string>("Unchanged")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Variation")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("YearHigh")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("YearLow")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("EntryDate")
                        .HasDatabaseName("IX_BroderMarketsIndex_EntryDate");

                    b.HasIndex("Key", "IndexSymbol", "EntryDate")
                        .HasDatabaseName("IX_BroderMarketsIndex_Symbol_EntryDate_Key");

                    b.HasIndex("Key", "IndexSymbol", "EntryDate", "Time")
                        .HasDatabaseName("IX_BroderMarketsIndex_Symbol_EntryDate_Time_Key");

                    b.ToTable("BroderMarkets");
                });

            modelBuilder.Entity("OptionChain.Controllers.SectorStocksResponse", b =>
                {
                    b.Property<double?>("Change")
                        .HasColumnType("float");

                    b.Property<double?>("DayHigh")
                        .HasColumnType("float");

                    b.Property<double?>("DayLow")
                        .HasColumnType("float");

                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<int>("IsNifty100")
                        .HasColumnType("int");

                    b.Property<int>("IsNifty200")
                        .HasColumnType("int");

                    b.Property<int>("IsNifty50")
                        .HasColumnType("int");

                    b.Property<double?>("LastPrice")
                        .HasColumnType("float");

                    b.Property<double?>("Open")
                        .HasColumnType("float");

                    b.Property<double?>("PChange")
                        .HasColumnType("float");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("TFactor")
                        .HasColumnType("float");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("SectorStocksResponse");
                });

            modelBuilder.Entity("OptionChain.Controllers.WeeklySectorUpdateParse", b =>
                {
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("PChange")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("WeekEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("WeekStartDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("WeeklyAverage")
                        .HasColumnType("decimal(18,2)");

                    b.ToTable("WeeklySectorUpdate");
                });

            modelBuilder.Entity("OptionChain.DailyStock", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<double>("Change")
                        .HasColumnType("float");

                    b.Property<string>("Chart30dPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Chart365dPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChartTodayPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date30dAgo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date365dAgo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("DayHigh")
                        .HasColumnType("float");

                    b.Property<double>("DayLow")
                        .HasColumnType("float");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Ffmc")
                        .HasColumnType("float");

                    b.Property<string>("Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("LastPrice")
                        .HasColumnType("float");

                    b.Property<string>("LastUpdateTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("NearWKH")
                        .HasColumnType("float");

                    b.Property<double>("NearWKL")
                        .HasColumnType("float");

                    b.Property<double>("Open")
                        .HasColumnType("float");

                    b.Property<double>("PChange")
                        .HasColumnType("float");

                    b.Property<double>("PreviousClose")
                        .HasColumnType("float");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("Series")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("StockIndClosePrice")
                        .HasColumnType("float");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<double>("TotalTradedValue")
                        .HasColumnType("float");

                    b.Property<long>("TotalTradedVolume")
                        .HasColumnType("bigint");

                    b.Property<double>("YearHigh")
                        .HasColumnType("float");

                    b.Property<double>("YearLow")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("EntryDate")
                        .HasDatabaseName("IX_DailyStockIndex_EntryDate");

                    b.HasIndex("Symbol", "EntryDate", "Time")
                        .HasDatabaseName("IX_DailyStockIndex_Symbol_EntryDate_Time");

                    b.ToTable("DailyStocks");
                });

            modelBuilder.Entity("OptionChain.FilteredOptionData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal>("AskPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("AskQty")
                        .HasColumnType("int");

                    b.Property<decimal>("BidPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("BidQty")
                        .HasColumnType("int");

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ChangeInOpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExpiryDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ImpliedVolatility")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PChange")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PChangeInOpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("StrikePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<int>("TotalBuyQuantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalSellQuantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalTradedVolume")
                        .HasColumnType("int");

                    b.Property<string>("Underlying")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UnderlyingValue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("EntryDate")
                        .HasDatabaseName("IX_FilteredOptionDataIndex_EntryDate");

                    b.ToTable("CurrentExpiryOptionDaata");
                });

            modelBuilder.Entity("OptionChain.Models.FiiDiiActivity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal?>("BuyValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("NetValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("SellValue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("FiiDiiActivitys");
                });

            modelBuilder.Entity("OptionChain.Models.IntradayBlast", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int>("Counter")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("LastPrice")
                        .HasColumnType("float");

                    b.Property<double>("PChange")
                        .HasColumnType("float");

                    b.Property<double>("PrevLastPrice")
                        .HasColumnType("float");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("Symbol", "EntryDate", "Time")
                        .HasDatabaseName("IX_IntradayBlastIndex_Symbol_EntryDate_Time");

                    b.ToTable("IntradayBlasts");
                });

            modelBuilder.Entity("OptionChain.Models.Users", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FamilyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GivenName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileImgeUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("VerifiedEmail")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OptionChain.OptionData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal>("AskPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("AskQty")
                        .HasColumnType("int");

                    b.Property<decimal>("BidPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("BidQty")
                        .HasColumnType("int");

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ChangeInOpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExpiryDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ImpliedVolatility")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LastPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PChange")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PChangeInOpenInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("StrikePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<int>("TotalBuyQuantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalSellQuantity")
                        .HasColumnType("int");

                    b.Property<int>("TotalTradedVolume")
                        .HasColumnType("int");

                    b.Property<string>("Underlying")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UnderlyingValue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("EntryDate")
                        .HasDatabaseName("IX_OptionDataIndex_EntryDate");

                    b.ToTable("AllOptionData");
                });

            modelBuilder.Entity("OptionChain.RFactorTable", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<double>("DayHigh")
                        .HasColumnType("float");

                    b.Property<double>("DayLow")
                        .HasColumnType("float");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("RFactor")
                        .HasColumnType("float");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("Symbol", "EntryDate", "Time")
                        .HasDatabaseName("IX_RFactorIndex_Symbol_EntryDate_Time");

                    b.ToTable("RFactors");
                });

            modelBuilder.Entity("OptionChain.Sector", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MappingName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sectors");
                });

            modelBuilder.Entity("OptionChain.Sessions", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Cookie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("OptionChain.StockData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<double>("Change")
                        .HasColumnType("float");

                    b.Property<string>("Chart30dPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Chart365dPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChartTodayPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date30dAgo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date365dAgo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("DayHigh")
                        .HasColumnType("float");

                    b.Property<double>("DayLow")
                        .HasColumnType("float");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Ffmc")
                        .HasColumnType("float");

                    b.Property<string>("Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("LastPrice")
                        .HasColumnType("float");

                    b.Property<string>("LastUpdateTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("NearWKH")
                        .HasColumnType("float");

                    b.Property<double>("NearWKL")
                        .HasColumnType("float");

                    b.Property<double>("Open")
                        .HasColumnType("float");

                    b.Property<double>("PChange")
                        .HasColumnType("float");

                    b.Property<double>("PreviousClose")
                        .HasColumnType("float");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("Series")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("StockIndClosePrice")
                        .HasColumnType("float");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<double>("TotalTradedValue")
                        .HasColumnType("float");

                    b.Property<long>("TotalTradedVolume")
                        .HasColumnType("bigint");

                    b.Property<double>("YearHigh")
                        .HasColumnType("float");

                    b.Property<double>("YearLow")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("EntryDate")
                        .HasDatabaseName("IX_StockDataIndex_EntryDate");

                    b.HasIndex("Symbol", "EntryDate", "Time")
                        .HasDatabaseName("IX_StockDataIndex_Symbol_EntryDate_Time");

                    b.ToTable("StockData");
                });

            modelBuilder.Entity("OptionChain.StockMetaData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCASec")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDebtSec")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelisted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsETFSec")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFNOSec")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMunicipalBond")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNifty100")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNifty200")
                        .HasColumnType("bit");

                    b.Property<bool>("IsNifty50")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSLBSec")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSuspended")
                        .HasColumnType("bit");

                    b.Property<string>("Isin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ListingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SlbIsin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("EntryDate")
                        .HasDatabaseName("IX_StockMetaDataIndex_EntryDate");

                    b.ToTable("StockMetaData");
                });

            modelBuilder.Entity("OptionChain.Summary", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<double>("CEPEOIDiff")
                        .HasColumnType("float");

                    b.Property<double>("CEPEOIPrevDiff")
                        .HasColumnType("float");

                    b.Property<double>("CEPEVolDiff")
                        .HasColumnType("float");

                    b.Property<double>("CEPEVolPrevDiff")
                        .HasColumnType("float");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time");

                    b.Property<double>("TotOICE")
                        .HasColumnType("float");

                    b.Property<double>("TotOIPE")
                        .HasColumnType("float");

                    b.Property<double>("TotVolCE")
                        .HasColumnType("float");

                    b.Property<double>("TotVolPE")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Summary");
                });
#pragma warning restore 612, 618
        }
    }
}
