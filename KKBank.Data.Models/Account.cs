using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Data.Models
{
    public class Account : BaseEntity
    {
        public Account()
        {
            this.PaymentOrdersList = new HashSet<PaymentOrder>();
            this.IncomesList = new HashSet<PaymentOrder>();
        }

        [Key]
        public int Id { get; set; } //Internal Number

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public int AccountTypeId { get; set; }

        public virtual AccountType AccountType { get; set; }

        public decimal Available { get; set; } // Наличност

        [Display(Name = "Blocked Аmount")]
        public decimal BlockedАmount { get; set; }

        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

        [MaxLength(22)]
        public string IBAN { get; set; }

        public decimal Balance 
        { 
            get
            {
                return this.Available + this.BlockedАmount;
            }
        }

        public virtual ICollection<PaymentOrder> PaymentOrdersList { get; set; } //Плащания

        public virtual ICollection<PaymentOrder> IncomesList { get; set; } //Постъпления
    }
}
