using System.ComponentModel.DataAnnotations;

namespace KKBank.Data.Models
{
    public class PaymentOrderStatus : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string StatusName { get; set; }
    }
}