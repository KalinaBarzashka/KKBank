using KKBank.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KKBank.Web.ViewModels.ViewModels.Payments
{
    public class PaymentsToKKBankAccountViewModel
    {
        public PaymentsToKKBankAccountViewModel()
        {
            this.Currency = new List<KeyValuePair<string, string>>();
            this.FromAccounts = new List<KeyValuePair<string, string>>();
        }

        public IEnumerable<KeyValuePair<string, string>> Currency { get; set; }

        [Required]
        [Range(1, Int64.MaxValue, ErrorMessage = "Please select from account!")]
        public int FromAccountId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> FromAccounts { get; set; }

        [Required(ErrorMessage = "Beneficiary's name is required!")]
        [MaxLength(64)]
        public string BeneficiaryName { get; set; }

        [Required(ErrorMessage = "IBAN field is required!")]
        [RegularExpression("^BG[0-9]{2}[A-Z]{4}[0-9]{4}[0-9]{2}[A-Z0-9]{8}$", ErrorMessage = "Incorrect format IBAN.")]
        [MaxLength(22)]
        public string IBAN { get; set; }

        [Required]
        [Range(1, Int64.MaxValue, ErrorMessage = "Please select transfer currency!")]
        public int TransferCurrencyId { get; set; }

        [Required]
        [MaxLength(14)]
        [RegularExpression("^[0-9]{1,11}[.|,]{0,1}[0-9]{0,2}$", ErrorMessage = "Please enter a valid amount greater than '0.00'. Valid amounts are from 1 to 11 digits, decimal point. Examples: '123.45', '6.15', '0.10'.")]
        [Range(0.01, 99999999999.99, ErrorMessage = "Please enter a valid amount greater than '0.00'. Valid amounts are from 1 to 14 digits, decimal point.")]
        public string AmountFrom { get; set; }

        [Required]
        [MaxLength(14)]
        [RegularExpression("^[0-9]{1,11}[.|,]{0,1}[0-9]{0,2}$", ErrorMessage = "Please enter a valid amount greater than '0.00'. Valid amounts are from 1 to 11 digits, decimal point. Examples: '123.45', '6.15', '0.10'.")]
        [Range(0.01, 99999999999.99, ErrorMessage = "Please enter a valid amount greater than '0.00'. Valid amounts are from 1 to 14 digits, decimal point.")]
        public string AmountTo { get; set; }

        [MaxLength(30, ErrorMessage = "Payment details must be less or equal to 30 symbols.")]
        [RegularExpression(".{1,30}", ErrorMessage = "Incorrect format.")]
        [Required(ErrorMessage = "Please enter details of the payment.")]
        public string PaymentDetails { get; set; }

        [MaxLength(30, ErrorMessage = "Additional details must be less or equal to 30 symbols.")]
        [RegularExpression(".{1,30}", ErrorMessage = "Incorrect format.")]
        public string AdditionalDetails { get; set; }
    }
}
