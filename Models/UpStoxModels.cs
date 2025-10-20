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
}
