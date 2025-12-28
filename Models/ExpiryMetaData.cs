using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OptionChain.Models
{
    public class ExpiryMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [JsonPropertyName("stockMetaDataId")]
        public long StockMetaDataId { get; set; }

        [JsonPropertyName("expiryDate")]
        public DateTime ExpiryDate { get; set; }

        [JsonPropertyName("isWeekly")]
        public bool IsWeekly { get; set; }
    }
}
