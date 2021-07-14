using System.ComponentModel.DataAnnotations;

namespace KKBank.Data.Models
{
    public class ToKKBankAccountsPaymentOrder : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int? FromAccountId { get; set; }

        public virtual Account FromAccount { get; set; }

        public int? ToAccountId { get; set; }

        public virtual Account ToAccount { get; set; }

        [Range(0.01, 99999999999.99, ErrorMessage = "Please enter a valid amount greater than '0.00'. Valid amounts are from 1 to 11 digits, decimal point. Examples: '123.45', '6.15', '0.10'.")]
        public decimal Amount { get; set; }

        public int TransferCurrencyId { get; set; }

        public virtual Currency TransferCurrency { get; set; }

        [Display(Name = "Details of Payment")]
        public string DetailsOfPayment { get; set; }

        public virtual PaymentOrderStatus PaymentOrderStatus { get; set; }

        public string PayorId { get; set; }
        public virtual ApplicationUser Payor { get; set; }

        public string PayeeId { get; set; }
        public virtual ApplicationUser Payee { get; set; }


        [Display(Name = "Beneficiary's name")]
        public string BeneficiaryName { get; set; }

        public string IBAN { get; set; }

        public virtual Currency ToAccountCurrency { get; set; }

        [Display(Name = "Additional details")]
        public string AdditionalDetails { get; set; }
    }
}
