using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptionChain
{
    public class OptionData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Identifier { get; set; }
        public double AskPrice { get; set; }
        public int AskQty { get; set; }
        public double BidPrice { get; set; }
        public int BidQty { get; set; }
        public double Change { get; set; }
        public double ChangeInOpenInterest { get; set; }
        public string? ExpiryDate { get; set; }
        public double ImpliedVolatility { get; set; }
        public double LastPrice { get; set; }
        public double OpenInterest { get; set; }
        public double PChange { get; set; }
        public double PChangeInOpenInterest { get; set; }
        public double StrikePrice { get; set; }
        public int TotalBuyQuantity { get; set; }
        public int TotalSellQuantity { get; set; }
        public int TotalTradedVolume { get; set; }
        public string? Underlying { get; set; }
        public double UnderlyingValue { get; set; }
        public DateTime? EntryDate { get; set; }
        public TimeSpan? Time { get; set; }
    }

    public class RFactorTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Symbol { get; set; }
        public double DayHigh { get; set; }
        public double DayLow { get; set; }
        public double Price { get; set; }
        public double RFactor { get; set; }
        public TimeSpan? Time { get; set; }
        public DateTime? EntryDate { get; set; }
    }

    public class FilteredOptionData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Identifier { get; set; }
        public double AskPrice { get; set; }
        public int AskQty { get; set; }
        public double BidPrice { get; set; }
        public int BidQty { get; set; }
        public double Change { get; set; }
        public double ChangeInOpenInterest { get; set; }
        public string? ExpiryDate { get; set; }
        public double ImpliedVolatility { get; set; }
        public double LastPrice { get; set; }
        public double OpenInterest { get; set; }
        public double PChange { get; set; }
        public double PChangeInOpenInterest { get; set; }
        public double StrikePrice { get; set; }
        public int TotalBuyQuantity { get; set; }
        public int TotalSellQuantity { get; set; }
        public int TotalTradedVolume { get; set; }
        public string? Underlying { get; set; }
        public double UnderlyingValue { get; set; }
        public DateTime? EntryDate { get; set; }
        public TimeSpan? Time { get; set; }

        public List<FilteredOptionData> ConvertToFilterOptionData(List<OptionData> optionDatas)
        {
            return optionDatas.Select(s => new FilteredOptionData
            {
                Identifier = s.Identifier,
                AskPrice = s.AskPrice,
                AskQty = s.AskQty,
                BidPrice = s.BidPrice,
                BidQty = s.BidQty,
                Change = s.Change,
                ChangeInOpenInterest = s.ChangeInOpenInterest,
                ExpiryDate = s.ExpiryDate,
                ImpliedVolatility = s.ImpliedVolatility,
                LastPrice = s.LastPrice,
                OpenInterest = s.OpenInterest,
                PChange = s.PChange,
                PChangeInOpenInterest = s.PChangeInOpenInterest,
                StrikePrice = s.StrikePrice,
                TotalBuyQuantity = s.TotalBuyQuantity,
                TotalSellQuantity = s.TotalSellQuantity,
                TotalTradedVolume = s.TotalTradedVolume,
                Underlying = s.Underlying,
                UnderlyingValue = s.UnderlyingValue,
                EntryDate = DateTime.Now.Date,
                Time = DateTime.Now.TimeOfDay
            }).ToList();
        }
    }

    public class Summary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public double TotOICE { get; set; }
        public double TotOIPE { get; set; }
        public double TotVolCE { get; set; }
        public double TotVolPE { get; set; }
        public double CEPEOIDiff { get; set; }
        public double CEPEVolDiff { get; set; }
        public double CEPEOIPrevDiff { get; set; }
        public double CEPEVolPrevDiff { get; set; }
        public TimeSpan? Time { get; set; }
        public DateTime? EntryDate { get; set; }

    }

    public class Advance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Declines { get; set; }
        public string? Advances { get; set; }
        public string? Unchanged { get; set; }
        public DateTime? EntryDate { get; set; }
        public TimeSpan? Time { get; set; }
    }

    public class StockData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public TimeSpan? Time { get; set; }
        public DateTime? EntryDate { get; set; }
    }

    public class StockMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
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
        public TimeSpan? Time { get; set; }
        public DateTime? EntryDate { get; set; }
    }

    public class Sector
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Symbol { get; set; }
        public string? MappingName { get; set; }
        public string? Industry { get; set; }
    }

    public class Sessions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Cookie { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    #region BANKNIFTY

    public class BankOptionData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Identifier { get; set; }
        public double AskPrice { get; set; }
        public int AskQty { get; set; }
        public double BidPrice { get; set; }
        public int BidQty { get; set; }
        public double Change { get; set; }
        public double ChangeInOpenInterest { get; set; }
        public string? ExpiryDate { get; set; }
        public double ImpliedVolatility { get; set; }
        public double LastPrice { get; set; }
        public double OpenInterest { get; set; }
        public double PChange { get; set; }
        public double PChangeInOpenInterest { get; set; }
        public double StrikePrice { get; set; }
        public int TotalBuyQuantity { get; set; }
        public int TotalSellQuantity { get; set; }
        public int TotalTradedVolume { get; set; }
        public string? Underlying { get; set; }
        public double UnderlyingValue { get; set; }
        public DateTime? EntryDate { get; set; }
        public TimeSpan? Time { get; set; }
    }

    public class BankExpiryOptionData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Identifier { get; set; }
        public double AskPrice { get; set; }
        public int AskQty { get; set; }
        public double BidPrice { get; set; }
        public int BidQty { get; set; }
        public double Change { get; set; }
        public double ChangeInOpenInterest { get; set; }
        public string? ExpiryDate { get; set; }
        public double ImpliedVolatility { get; set; }
        public double LastPrice { get; set; }
        public double OpenInterest { get; set; }
        public double PChange { get; set; }
        public double PChangeInOpenInterest { get; set; }
        public double StrikePrice { get; set; }
        public int TotalBuyQuantity { get; set; }
        public int TotalSellQuantity { get; set; }
        public int TotalTradedVolume { get; set; }
        public string? Underlying { get; set; }
        public double UnderlyingValue { get; set; }
        public DateTime? EntryDate { get; set; }
        public TimeSpan? Time { get; set; }

        public List<BankExpiryOptionData> ConvertToFilterOptionData(List<BankOptionData> optionDatas)
        {
            return optionDatas.Select(s => new BankExpiryOptionData
            {
                Identifier = s.Identifier,
                AskPrice = s.AskPrice,
                AskQty = s.AskQty,
                BidPrice = s.BidPrice,
                BidQty = s.BidQty,
                Change = s.Change,
                ChangeInOpenInterest = s.ChangeInOpenInterest,
                ExpiryDate = s.ExpiryDate,
                ImpliedVolatility = s.ImpliedVolatility,
                LastPrice = s.LastPrice,
                OpenInterest = s.OpenInterest,
                PChange = s.PChange,
                PChangeInOpenInterest = s.PChangeInOpenInterest,
                StrikePrice = s.StrikePrice,
                TotalBuyQuantity = s.TotalBuyQuantity,
                TotalSellQuantity = s.TotalSellQuantity,
                TotalTradedVolume = s.TotalTradedVolume,
                Underlying = s.Underlying,
                UnderlyingValue = s.UnderlyingValue,
                EntryDate = DateTime.Now.Date,
                Time = DateTime.Now.TimeOfDay
            }).ToList();
        }
    }

    public class BankSummary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public double TotOICE { get; set; }
        public double TotOIPE { get; set; }
        public double TotVolCE { get; set; }
        public double TotVolPE { get; set; }
        public double CEPEOIDiff { get; set; }
        public double CEPEVolDiff { get; set; }
        public double CEPEOIPrevDiff { get; set; }
        public double CEPEVolPrevDiff { get; set; }
        public TimeSpan? Time { get; set; }
        public DateTime? EntryDate { get; set; }

    }

    #endregion

    #region "Parser Models Nifty"
    public class Record
    {
        public long Id { get; set; }
        public List<OptionData> Data { get; set; }
        public List<string> ExpiryDates { get; set; }
    }

    public class Filtered
    {
        public long Id { get; set; }
        public List<OptionData> Data { get; set; }
        public OptionSummary CE { get; set; }
        public OptionSummary PE { get; set; }
    }

    public class OptionSummary
    {
        public double TotOI { get; set; }
        public double TotVol { get; set; }
    }

    public class Root
    {
        public Record Records { get; set; }
        public Filtered Filtered { get; set; }
    }
    #endregion

    #region "Parse Models BankNifty"
    public class BankRecord
    {
        public long Id { get; set; }
        public List<BankOptionData> Data { get; set; }
        public List<string> ExpiryDates { get; set; }
    }

    public class BankFiltered
    {
        public long Id { get; set; }
        public List<BankOptionData> Data { get; set; }
        public OptionSummary CE { get; set; }
        public OptionSummary PE { get; set; }
    }

    public class BankRoot
    {
        public BankRecord Records { get; set; }
        public BankFiltered Filtered { get; set; }
    }
    #endregion

}
