using Microsoft.EntityFrameworkCore;
using OptionChain.Controllers;
using OptionChain.Models;

namespace OptionChain
{
    public class OptionDbContext : DbContext
    {
        public DbSet<OptionData> AllOptionData { get; set; }
        public DbSet<FilteredOptionData> CurrentExpiryOptionDaata { get; set; }
        public DbSet<Summary> Summary { get; set; }
        public DbSet<Advance> Advance { get; set; }
        public DbSet<StockData> StockData { get; set; }
        public DbSet<DailyStockData> DailyStockData { get; set; }
        public DbSet<StockMetaData> StockMetaData { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Sessions> Sessions { get; set; }
        public DbSet<RFactorTable> RFactors { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<FiiDiiActivity> FiiDiiActivitys { get; set; }

        // Bank NIFTY Options
        public DbSet<BankOptionData> BankOptionData { get; set; }
        public DbSet<BankExpiryOptionData> BankExpiryOptionData { get; set; }
        public DbSet<BankSummary> BankSummary { get; set; }
        public DbSet<IntradayBlast> IntradayBlasts { get; set; }

        // Broder Index
        public DbSet<BroderMarkets> BroderMarkets { get; set; }

        // SP Execution
        public DbSet<SectorStocksResponse> SameOpenLowHigh { get; set; }
        public DbSet<WeeklySectorUpdateParse> WeeklySectorUpdate { get; set; }
        public DbSet<SectorStocksResponse> WeeklyStockUpdates { get; set; }
        public DbSet<ResponseSectorsStocks> ResponseSectorsStocks { get; set; }
        public DbSet<BreakoutStock> BreakoutStocks { get; set; }
        public DbSet<OptionValues> OptionValues { get; set; }

        // Constructor for DbContext
        public OptionDbContext(DbContextOptions<OptionDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the primary keys and relationships
            modelBuilder.Entity<OptionData>()
                .HasKey(o => o.Id); // OptionData Primary Key

            modelBuilder.Entity<IntradayBlast>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<IntradayBlast>()
                .HasIndex(o => new { o.Symbol, o.EntryDate, o.Time }).HasDatabaseName("IX_IntradayBlastIndex_Symbol_EntryDate_Time");

            modelBuilder.Entity<OptionData>()
                .HasIndex(o => o.EntryDate).HasDatabaseName("IX_OptionDataIndex_EntryDate");

            modelBuilder.Entity<FilteredOptionData>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<FilteredOptionData>()
                .HasIndex(o => o.EntryDate).HasDatabaseName("IX_FilteredOptionDataIndex_EntryDate");

            modelBuilder.Entity<Summary>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<BankOptionData>()
                .HasKey(o => o.Id); // OptionData Primary Key

            modelBuilder.Entity<BankOptionData>()
                .HasIndex(o => o.EntryDate).HasDatabaseName("IX_BankOptionDataIndex_EntryDate");

            modelBuilder.Entity<BankExpiryOptionData>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<BankExpiryOptionData>()
                .HasIndex(o => o.EntryDate).HasDatabaseName("IX_BankExpiryOptionDataIndex_EntryDate");

            modelBuilder.Entity<BankSummary>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<Advance>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<StockData>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<StockData>()
                .HasIndex(o => o.Symbol).HasDatabaseName("IX_StockDataIndex_Symbol");

            modelBuilder.Entity<StockData>()
                .HasIndex(o => o.EntryDate).HasDatabaseName("IX_StockDataIndex_EntryDate");
            
            modelBuilder.Entity<StockData>()
                .HasIndex(o => new { o.Symbol, o.EntryDate, o.Time }).HasDatabaseName("IX_StockDataIndex_Symbol_EntryDate_Time");

            modelBuilder.Entity<DailyStockData>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<DailyStockData>()
                .HasIndex(s => s.Symbol).HasDatabaseName("IX_DailyStockDataIndex_Symbol");

            modelBuilder.Entity<DailyStockData>()
                .HasIndex(s => s.EntryDate).HasDatabaseName("IX_DailyStockDataIndex_EntryDate");

            modelBuilder.Entity<StockMetaData>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<RFactorTable>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<RFactorTable>()
                .HasIndex(o => o.Symbol).HasDatabaseName("IX_RFactorIndex_Symbol");

            modelBuilder.Entity<RFactorTable>()
                .HasIndex(o => o.EntryDate).HasDatabaseName("IX_RFactorIndex_EntryDate");

            modelBuilder.Entity<RFactorTable>()
                .HasIndex(o => new { o.Symbol, o.EntryDate, o.Time }).HasDatabaseName("IX_RFactorIndex_Symbol_EntryDate_Time");

            modelBuilder.Entity<Users>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<RFactorTable>()
                .HasIndex(o => new { o.Symbol, o.EntryDate, o.Time }).HasDatabaseName("IX_RFactorIndex_Symbol_EntryDate_Time");

            modelBuilder.Entity<StockMetaData>()
                .HasIndex(o => o.EntryDate).HasDatabaseName("IX_StockMetaDataIndex_EntryDate");

            modelBuilder.Entity<Sector>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<BroderMarkets>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<BroderMarkets>()
                .HasIndex(o => o.EntryDate).HasDatabaseName("IX_BroderMarketsIndex_EntryDate");

            modelBuilder.Entity<BroderMarkets>()
                .HasIndex(o => new { o.Key, o.IndexSymbol, o.EntryDate, o.Time }).HasDatabaseName("IX_BroderMarketsIndex_Symbol_EntryDate_Time_Key");

            modelBuilder.Entity<BroderMarkets>()
                .HasIndex(o => new { o.Key, o.IndexSymbol, o.EntryDate }).HasDatabaseName("IX_BroderMarketsIndex_Symbol_EntryDate_Key");

            modelBuilder.Entity<SectorStocksResponse>().HasNoKey();
            modelBuilder.Entity<WeeklySectorUpdateParse>().HasNoKey();
            modelBuilder.Entity<ResponseSectorsStocks>().HasNoKey();
            modelBuilder.Entity<BreakoutStock>().HasNoKey();
            modelBuilder.Entity<OptionValues>().HasNoKey();

            modelBuilder.Entity<SectorStocksResponse>()
                .Property(e => e.IsNifty50)
                .HasConversion(
                    v => v ? 1 : 0, // Convert bool to int when saving
                    v => v == 1      // Convert int to bool when reading
                );

            modelBuilder.Entity<SectorStocksResponse>()
                .Property(e => e.IsNifty100)
                .HasConversion(
                    v => v ? 1 : 0, // Convert bool to int when saving
                    v => v == 1      // Convert int to bool when reading
                );
            modelBuilder.Entity<SectorStocksResponse>()
                .Property(e => e.IsNifty200)
                .HasConversion(
                    v => v ? 1 : 0, // Convert bool to int when saving
                    v => v == 1      // Convert int to bool when reading
                );
        }
    }

    public class UpStoxDbContext: DbContext
    {
        public DbSet<OHLC> OHLCs { get; set; }
        public DbSet<AuthDetails> AuthDetails{ get; set; }
        public DbSet<MarketMetaData> MarketMetaDatas{ get; set; }
        public DbSet<SectorStockMetaData> SectorStockMetaDatas { get; set; }
        public DbSet<PreComputedData> PreComputedDatas { get; set; }
        public DbSet<FuturePreComputedData> FuturePreComputedDatas { get; set; }

        public UpStoxDbContext(DbContextOptions<UpStoxDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OHLC>().HasKey(o => o.Id);
            modelBuilder.Entity<OHLC>().Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<OHLC>().Property(e => e.Time).HasDefaultValueSql("CONVERT(VARCHAR(5), GETUTCDATE(), 108)");
            modelBuilder.Entity<OHLC>().Property(x => x.PChange).HasPrecision(10, 2);
            modelBuilder.Entity<OHLC>().Property(x => x.LastPrice).HasPrecision(10, 2);
            modelBuilder.Entity<OHLC>().Property(x => x.Open).HasPrecision(10, 2);
            modelBuilder.Entity<OHLC>().Property(x => x.High).HasPrecision(10, 2);
            modelBuilder.Entity<OHLC>().Property(x => x.Low).HasPrecision(10, 2);
            modelBuilder.Entity<OHLC>().Property(x => x.Close).HasPrecision(10, 2);

            modelBuilder.Entity<OHLC>()
                .HasIndex(o => new { o.CreatedDate, o.Time }).HasDatabaseName("IX_OHLCs_CreatedDate_Time");
            modelBuilder.Entity<OHLC>()
                .HasIndex(o => new { o.CreatedDate, o.Time, o.StockMetaDataId }).HasDatabaseName("IX_OHLCs_CreatedDate_Time_StockMetaDataId");
            modelBuilder.Entity<OHLC>()
                .HasIndex(o => new { o.CreatedDate, o.StockMetaDataId }).HasDatabaseName("IX_OHLCs_CreatedDate_StockMetaDataId");


            modelBuilder.Entity<AuthDetails>().HasKey(o => o.Id);
            modelBuilder.Entity<AuthDetails>().Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<MarketMetaData>().HasKey(o => o.Id);
            modelBuilder.Entity<MarketMetaData>().Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<SectorStockMetaData>().HasKey(o => o.Id);

            modelBuilder.Entity<PreComputedData>().HasKey(x => x.Id);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysHigh).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysLow).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysAverageClose).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysVWAP).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysStdDevClose).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysStdDevVolume).HasPrecision(18, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysAverageVolume).HasPrecision(18, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysGreenPercentage).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysAboveVWAPPercentage).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysHighLowRangePercentage).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.DaysAverageBodySize).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.PreviousDayHigh).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.PreviousDayLow).HasPrecision(10, 2);
            modelBuilder.Entity<PreComputedData>().Property(x => x.PreviousDayClose).HasPrecision(10, 2);

            modelBuilder.Entity<FuturePreComputedData>().Property(x => x.PivotPoint).HasPrecision(10, 2);
            modelBuilder.Entity<FuturePreComputedData>().Property(x => x.BottomCP).HasPrecision(10, 2);
            modelBuilder.Entity<FuturePreComputedData>().Property(x => x.TopCP).HasPrecision(10, 2);
        }
    }
}
