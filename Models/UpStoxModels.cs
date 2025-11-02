using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OptionChain.Models
{
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
        public long? DaysATR { get; set; }

        [JsonPropertyName("daysMedianATR")]
        public long? DaysMedianATR { get; set; }

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
}
