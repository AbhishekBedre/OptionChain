﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OptionChain;

#nullable disable

namespace OptionChain.Migrations
{
    [DbContext(typeof(OptionDbContext))]
    partial class OptionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("OptionChain.FilteredOptionData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<double>("AskPrice")
                        .HasColumnType("float");

                    b.Property<int>("AskQty")
                        .HasColumnType("int");

                    b.Property<double>("BidPrice")
                        .HasColumnType("float");

                    b.Property<int>("BidQty")
                        .HasColumnType("int");

                    b.Property<double>("Change")
                        .HasColumnType("float");

                    b.Property<double>("ChangeInOpenInterest")
                        .HasColumnType("float");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExpiryDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ImpliedVolatility")
                        .HasColumnType("float");

                    b.Property<double>("LastPrice")
                        .HasColumnType("float");

                    b.Property<double>("OpenInterest")
                        .HasColumnType("float");

                    b.Property<double>("PChange")
                        .HasColumnType("float");

                    b.Property<double>("PChangeInOpenInterest")
                        .HasColumnType("float");

                    b.Property<double>("StrikePrice")
                        .HasColumnType("float");

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

                    b.Property<double>("UnderlyingValue")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("CurrentExpiryOptionDaata");
                });

            modelBuilder.Entity("OptionChain.OptionData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<double>("AskPrice")
                        .HasColumnType("float");

                    b.Property<int>("AskQty")
                        .HasColumnType("int");

                    b.Property<double>("BidPrice")
                        .HasColumnType("float");

                    b.Property<int>("BidQty")
                        .HasColumnType("int");

                    b.Property<double>("Change")
                        .HasColumnType("float");

                    b.Property<double>("ChangeInOpenInterest")
                        .HasColumnType("float");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExpiryDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ImpliedVolatility")
                        .HasColumnType("float");

                    b.Property<double>("LastPrice")
                        .HasColumnType("float");

                    b.Property<double>("OpenInterest")
                        .HasColumnType("float");

                    b.Property<double>("PChange")
                        .HasColumnType("float");

                    b.Property<double>("PChangeInOpenInterest")
                        .HasColumnType("float");

                    b.Property<double>("StrikePrice")
                        .HasColumnType("float");

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

                    b.Property<double>("UnderlyingValue")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("AllOptionData");
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
                        .HasColumnType("nvarchar(max)");

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
