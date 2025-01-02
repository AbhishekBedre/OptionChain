using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

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
                EntryDate = DateTime.Now.Date
            }).ToList();
        }
    }

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
        public string? Time { get; set; }
        public DateTime? EntryDate { get; set; }

    }
}
