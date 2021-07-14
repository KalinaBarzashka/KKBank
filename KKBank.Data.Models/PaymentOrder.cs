using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KKBank.Data.Models
{
    public class PaymentOrder : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int? FromAccountId { get; set; }

        public virtual Account FromAccount { get; set; }

        public int? ToAccountId { get; set; }

        public virtual Account ToAccount { get; set; }

        [Range(0.01, 99999999999.99, ErrorMessage = "Please enter a valid amount greater than '0.00'. Valid amounts are from 1 to 11 digits, decimal point. Examples: '123.45', '6.15', '0.10'.")]
        public decimal FromAmount { get; set; }

        [Range(0.01, 99999999999.99, ErrorMessage = "Please enter a valid amount greater than '0.00'. Valid amounts are from 1 to 11 digits, decimal point. Examples: '123.45', '6.15', '0.10'.")]
        public decimal ToAmount { get; set; }

        public int CurrencyFromId { get; set; }

        public virtual Currency CurrencyFrom { get; set; }

        public int CurrencyToId { get; set; }

        public virtual Currency CurrencyTo { get; set; }

        public int TransferCurrencyId { get; set; }

        public virtual Currency TransferCurrency { get; set; }

        [Display(Name = "Details of Payment")]
        [MaxLength(30)]
        public string DetailsOfPayment { get; set; }

        public virtual int? PaymentOrderStatusId { get; set; }

        public virtual PaymentOrderStatus PaymentOrderStatus { get; set; }

        public string PayorId { get; set; }
        public virtual ApplicationUser Payor { get; set; }

        public string PayeeId { get; set; }
        public virtual ApplicationUser Payee { get; set; }

        [Display(Name = "Beneficiary's name")]
        [MaxLength(64)]
        public string BeneficiaryName { get; set; }

        [MaxLength(22)]
        public string IBAN { get; set; }

        [Display(Name = "Additional details")]
        [MaxLength(30)]
        public string AdditionalDetails { get; set; }
    }
}
