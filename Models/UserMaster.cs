using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OptionChain.Models
{
    public class UserMaster
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
}
