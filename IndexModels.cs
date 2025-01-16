using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptionChain
{
    public class BroderMarkets
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Key { get; set; }
        public string? Index { get; set; }
        public string? IndexSymbol { get; set; }
        public decimal Last { get; set; }
        public decimal Variation { get; set; }
        public decimal PercentChange { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal YearHigh { get; set; }
        public decimal YearLow { get; set; }
        public decimal IndicativeClose { get; set; }

        [MaxLength(10)]
        public string? PE { get; set; } // Keeping it as string since it's a ratio

        [MaxLength(10)]
        public string? PB { get; set; } // Keeping it as string for consistency

        [MaxLength(10)]
        public string? DY { get; set; } // Keeping it as string for consistency

        public string? Declines { get; set; }
        public string? Advances { get; set; }
        public string? Unchanged { get; set; }
        public string? Date365dAgo { get; set; } // Converted to DateTime
        public string? Chart365dPath { get; set; }
        public string? Date30dAgo { get; set; } // Converted to DateTime
        public decimal? PerChange30d { get; set; }
        public string? Chart30dPath { get; set; }

        public decimal? PreviousDay { get; set; }
        public decimal? OneWeekAgo { get; set; }
        public decimal? OneMonthAgo { get; set; }
        public decimal? OneYearAgo { get; set; }
        public DateTime? EntryDate { get; set; }
        public TimeSpan? Time { get; set; }
    }

    // JSON Parser Model
    public class BroderMarketRoot
    {
        public List<BroderMarkets> Data { get; set; }
    }
}