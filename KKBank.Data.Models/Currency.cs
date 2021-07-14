using System.ComponentModel.DataAnnotations;

namespace KKBank.Data.Models
{
    public class Currency : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string CurrencyName { get; set; }

        [Required]
        [MaxLength(3)]
        public string CurrencyAbbreviation { get; set; }
    }
}