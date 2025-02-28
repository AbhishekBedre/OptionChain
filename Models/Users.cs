using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptionChain.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? ProfileImgeUrl { get; set; }
        public bool VerifiedEmail { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class FiiDiiActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Category { get; set; }
        public string? Date { get; set; }
        public decimal? BuyValue { get; set; }
        public decimal? SellValue { get; set; }
        public decimal? NetValue { get; set; }
    }
}
