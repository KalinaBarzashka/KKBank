using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Web.ViewModels.ViewModels.Payments
{
    public class PaymentsBetweenOwnAccountsViewModel
    {
        public PaymentsBetweenOwnAccountsViewModel()
        {
            this.FromAccounts = new List<KeyValuePair<string, string>>();
            this.ToAccounts = new List<KeyValuePair<string, string>>();
        }

        [Required]
        [Range(1, Int64.MaxValue, ErrorMessage = "Please select from account!")]
        public int FromAccountId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> FromAccounts { get; set; }

        [Required]
        [Range(1, Int64.MaxValue, ErrorMessage = "Please select to account!")]
        public int ToAccountId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToAccounts { get; set; }

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
    }
}
