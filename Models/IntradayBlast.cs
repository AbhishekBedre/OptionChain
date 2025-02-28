using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OptionChain.Models
{
    public class IntradayBlast
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Symbol { get; set; }
        public double LastPrice { get; set; }
        public double PrevLastPrice { get; set; }
        public double PChange { get; set; }
        public TimeSpan? Time { get; set; }
        public DateTime? EntryDate { get; set; }
        public int Counter { get; set; }
    }
}
