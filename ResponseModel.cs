using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OptionChain
{
    public class StockDataParse
    {
        public long Id { get; set; }
        public int Priority { get; set; }
        public string? Symbol { get; set; }
        public string? Identifier { get; set; }
        public string? Series { get; set; }
        public double Open { get; set; }
        public double DayHigh { get; set; }
        public double DayLow { get; set; }
        public double LastPrice { get; set; }
        public double PreviousClose { get; set; }
        public double Change { get; set; }
        public double PChange { get; set; }
        public long TotalTradedVolume { get; set; }
        public double StockIndClosePrice { get; set; }
        public double TotalTradedValue { get; set; }
        public string? LastUpdateTime { get; set; }
        public double YearHigh { get; set; }
        public double Ffmc { get; set; }
        public double YearLow { get; set; }
        public double NearWKH { get; set; }
        public double NearWKL { get; set; }

        //[JsonConverter(typeof(IntOrDashConverter))]
        //public double PerChange365d { get; set; }
        public string? Date365dAgo { get; set; }
        public string? Chart365dPath { get; set; }
        public string? Date30dAgo { get; set; }
        //public double PerChange30d { get; set; }
        public string? Chart30dPath { get; set; }
        public string? ChartTodayPath { get; set; }
        public DateTime? EntryDate { get; set; }
        public StockMetaDataParser? Meta { get; set; }
    }

    public class StockMetaDataParser
    {
        public string? Symbol { get; set; }
        public string? CompanyName { get; set; }
        public string? Industry { get; set; }
        public bool IsFNOSec { get; set; }
        public bool IsCASec { get; set; }
        public bool IsSLBSec { get; set; }
        public bool IsDebtSec { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsETFSec { get; set; }
        public bool IsDelisted { get; set; }
        public string? Isin { get; set; }
        public string? SlbIsin { get; set; }
        public DateTime ListingDate { get; set; }
        public bool IsMunicipalBond { get; set; }
        public DateTime? EntryDate { get; set; }
    }

    public class StockRoot
    {
        public string Name { get; set; }
        public Advance Advance { get; set; }
        public List<StockDataParse> Data { get; set; }
    }
}
