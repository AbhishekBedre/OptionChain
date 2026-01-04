using System.Text.Json.Serialization;

namespace OptionChain.Models
{
    public class OHLCResponse
    {
        [JsonPropertyName("open")]
        public decimal Open { get; set; }

        [JsonPropertyName("high")]
        public decimal High { get; set; }

        [JsonPropertyName("low")]
        public decimal Low { get; set; }

        [JsonPropertyName("close")]
        public decimal Close { get; set; }
    }
}
