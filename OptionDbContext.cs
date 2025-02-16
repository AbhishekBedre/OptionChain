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

        // Broder Index
        public DbSet<BroderMarkets> BroderMarkets { get; set; }

        // SP Execution
        public DbSet<SectorStocksResponse> SameOpenLowHigh { get; set; }
        public DbSet<WeeklySectorUpdateParse> WeeklySectorUpdate { get; set; }
        public DbSet<SectorStocksResponse> WeeklyStockUpdates { get; set; }

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
                .HasIndex(o => o.EntryDate).HasDatabaseName("IX_StockDataIndex_EntryDate");

            modelBuilder.Entity<StockData>()
                .HasIndex(o => new { o.Symbol, o.EntryDate, o.Time }).HasDatabaseName("IX_StockDataIndex_Symbol_EntryDate_Time");

            modelBuilder.Entity<StockMetaData>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<RFactorTable>()
                .HasKey(s => s.Id); //PK

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

            modelBuilder.Entity<SectorStocksResponse>().HasNoKey();
            modelBuilder.Entity<WeeklySectorUpdateParse>().HasNoKey();
        }
    }

}
