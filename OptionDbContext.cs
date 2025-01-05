using Microsoft.EntityFrameworkCore;

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

            modelBuilder.Entity<FilteredOptionData>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<Summary>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<Advance>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<StockData>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<StockMetaData>()
                .HasKey(s => s.Id); //PK

            modelBuilder.Entity<Sector>()
                .HasKey(s => s.Id); //PK
        }
    }

}
