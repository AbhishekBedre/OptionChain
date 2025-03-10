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
    [Migration("20250102030807_EntryDateCol")]
    partial class EntryDateCol
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OptionChain.FilteredOptionData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

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

            modelBuilder.Entity("OptionChain.Summary", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

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

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

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
