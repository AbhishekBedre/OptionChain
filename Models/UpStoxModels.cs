using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OptionChain.Models
{ 
    #region OptionDataModel
    public class ApiOptionResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public List<OptionDataItem> Data { get; set; }
    }
    public class OptionDataItem
    {
        [JsonPropertyName("expiry")]
        public DateTime Expiry { get; set; }

        [JsonPropertyName("pcr")]
        public decimal PCR { get; set; }

        [JsonPropertyName("strike_price")]
        public decimal StrikePrice { get; set; }

        [JsonPropertyName("underlying_spot_price")]
        public decimal UnderlyingSpotPrice { get; set; }

        [JsonPropertyName("call_options")]
        public OptionSide CallOptions { get; set; }

        [JsonPropertyName("put_options")]
        public OptionSide PutOptions { get; set; }
    }

    public class OptionSide
    {
        [JsonPropertyName("instrument_key")]
        public string InstrumentKey { get; set; }

        [JsonPropertyName("market_data")]
        public MarketData MarketData { get; set; }
    }

    public class MarketData
    {
        [JsonPropertyName("ltp")]
        public decimal LTP { get; set; }

        [JsonPropertyName("volume")]
        public long Volume { get; set; }

        [JsonPropertyName("oi")]
        public decimal OI { get; set; }

        [JsonPropertyName("close_price")]
        public decimal ClosePrice { get; set; }

        [JsonPropertyName("prev_oi")]
        public decimal PrevOI { get; set; }
    }

    // OptionData table
    public class OptionExpiryData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("stockMetaDataId")]
        public long StockMetaDataId { get; set; }

        [JsonPropertyName("expiry")]
        public DateTime Expiry { get; set; }

        [JsonPropertyName("strikePrice")]
        public decimal StrikePrice { get; set; }

        [JsonPropertyName("strikePCR")]
        public decimal StrikePCR { get; set; }

        [JsonPropertyName("spotPrice")]
        public decimal SpotPrice { get; set; }


        [JsonPropertyName("callOI")]
        public long CallOI { get; set; }

        [JsonPropertyName("callLTP")]
        public decimal CallLTP { get; set; }

        [JsonPropertyName("callVolume")]
        public long CallVolume { get; set; }

        [JsonPropertyName("callPrevOI")]
        public long CallPrevOI { get; set; }


        [JsonPropertyName("putOI")]
        public long PutOI { get; set; }

        [JsonPropertyName("putLTP")]
        public decimal PutLTP { get; set; }

        [JsonPropertyName("putVolume")]
        public long PutVolume { get; set; }

        [JsonPropertyName("putPrevOI")]
        public long PutPrevOI { get; set; }

        // Positive => market down, Negative => market up
        [JsonPropertyName("callPutDiff")]
        public long CallPutDiff => CallOI - PutOI;

        [JsonPropertyName("openContractChange")]
        public long OpenContractChange { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("time")]
        public TimeSpan? Time { get; set; }
    }

    public class OptionExpirySummary
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

    public class ApiResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, StockData> Data { get; set; }
    }

    public class StockData
    {
        [JsonPropertyName("last_price")]
        public decimal LastPrice { get; set; }

        [JsonPropertyName("instrument_token")]
        public string InstrumentToken { get; set; }

        [JsonPropertyName("prev_ohlc")]
        public OHLC PrevOhlc { get; set; }

        [JsonPropertyName("live_ohlc")]
        public OHLC LiveOhlc { get; set; }
    }

    public class OHLC
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("stockMerataDataId")]
        public long StockMetaDataId { get; set; }

        [JsonPropertyName("open")]
        public decimal Open { get; set; }

        [JsonPropertyName("high")]
        public decimal High { get; set; }

        [JsonPropertyName("low")]
        public decimal Low { get; set; }

        [JsonPropertyName("close")]
        public decimal Close { get; set; }

        [JsonPropertyName("volume")]
        public long Volume { get; set; }

        [JsonPropertyName("ts")]
        public long Timestamp { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonPropertyName("time")]
        public TimeSpan? Time { get; set; }

        [JsonPropertyName("lastPrice")]
        public decimal? LastPrice { get; set; }

        [JsonPropertyName("pChange")]
        public decimal? PChange { get; set; }

        [JsonPropertyName("rFactor")]
        public decimal? RFactor { get; set; }
    }

    public class MarketMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("instrumentToken")]
        public string InstrumentToken { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonPropertyName("modifiedDate")]
        public DateTime? ModifiedDate { get; set; }
    }

    public class SectorStockMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("stockMetaDataId")]
        public long StockMetaDataId { get; set; }

        [JsonPropertyName("sectorId")]
        public long SectorId { get; set; }

        [JsonPropertyName("sectorDisplayName")]
        public string SectorDisplayName { get; set; }
    }

    public class AuthDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("authCode")]
        public string AuthCode { get; set; }

        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("clientSecret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("redirectUrl")]
        public string RedirectUrl { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonPropertyName("modifiedDate")]
        public DateTime? ModifiedDate { get; set; }
    }

    public class PreComputedData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("stockMetaDataId")]
        public long StockMetaDataId { get; set; }

        // Will use 10 days high
        [JsonPropertyName("daysHigh")]
        public decimal? DaysHigh { get; set; }

        // will use 10 days low
        [JsonPropertyName("daysLow")]
        public decimal? DaysLow { get; set; }

        [JsonPropertyName("daysAverageClose")]
        public decimal? DaysAverageClose { get; set; }

        [JsonPropertyName("daysAverageVolume")]
        public long? DaysAverageVolume { get; set; }

        [JsonPropertyName("daysATR")]
        public decimal? DaysATR { get; set; }

        [JsonPropertyName("daysMedianATR")]
        public decimal? DaysMedianATR { get; set; }

        // Trend Score 10-Day (Up/Down/Side)
        [JsonPropertyName("daysTrendScore")]
        public int DaysTrendScore { get; set; }

        [JsonPropertyName("daysVWAP")]
        public decimal? DaysVWAP { get; set; }

        [JsonPropertyName("daysStdDevClose")]
        public decimal? DaysStdDevClose { get; set; }

        [JsonPropertyName("daysStdDevVolume")]
        public decimal? DaysStdDevVolume { get; set; }

        [JsonPropertyName("daysGreenPercentage")]
        public decimal? DaysGreenPercentage { get; set; } // 100 - Green% = Red %

        [JsonPropertyName("daysAboveVWAPPercentage")]
        public decimal? DaysAboveVWAPPercentage { get; set; }

        [JsonPropertyName("daysHighLowRangePercentage")]
        public decimal? DaysHighLowRangePercentage { get; set; }

        [JsonPropertyName("daysAverageBodySize")]
        public decimal? DaysAverageBodySize { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonPropertyName("previousDayHigh")]
        public decimal? PreviousDayHigh { get; set; }

        [JsonPropertyName("previousDayLow")]
        public decimal? PreviousDayLow { get; set; }

        [JsonPropertyName("previousDayClose")]
        public decimal? PreviousDayClose { get; set; }
    }

    public class FuturePreComputedData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("stockMetaDataId")]
        public long StockMetaDataId { get; set; }

        [JsonPropertyName("pivotPoint")]
        public decimal? PivotPoint { get; set; }

        [JsonPropertyName("bottomCP")]
        public decimal? BottomCP { get; set; }

        [JsonPropertyName("topCP")]
        public decimal? TopCP { get; set; }

        [JsonPropertyName("tr1")]
        public bool TR1 { get; set; } = false;

        [JsonPropertyName("tr2")]
        public bool TR2 { get; set; } = false;

        [JsonPropertyName("wasTrendy")]
        public bool WasTrendy { get; set; } = false;

        [JsonPropertyName("createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonPropertyName("modifiedDate")]
        public DateTime? ModifiedDate { get; set; }

        [JsonPropertyName("forDate")]
        public DateTime? ForDate { get; set; }
    }

    public class BreakOutDownStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("stockMerataDataId")]
        public long StockMetaDataId { get; set; }

        [JsonPropertyName("time")]
        public TimeSpan? Time { get; set; }

        [JsonPropertyName("lastPrice")]
        public decimal? LastPrice { get; set; }

        [JsonPropertyName("pChange")]
        public decimal? PChange { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonPropertyName("trend")]
        public bool Trend { get; set; }

    }

}
