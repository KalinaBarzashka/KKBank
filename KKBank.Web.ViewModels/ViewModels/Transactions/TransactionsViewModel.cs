using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Web.ViewModels.ViewModels.Transactions
{
    public class TransactionsViewModel
    {
        public TransactionsViewModel()
        {
            this.Transactions = new List<TransactionViewModel>();
            this.TransactionTypes = new List<KeyValuePair<string, string>>();
            this.Accounts = new List<KeyValuePair<int, string>>();
        }

        public IEnumerable<TransactionViewModel> Transactions { get; set; }

        [Range(0, 2)]
        public int TransactionTypeId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> TransactionTypes { get; set; }

        public int AccountsId { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Accounts { get; set; }

        public string FilerType { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "The field must contain only digits!")]
        [RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Please enter a number for Last N transaction!")]
        [MaxLength(3)]
        public string LastNRequests { get; set; }

        public string ToDate { get; set; }

        [MaxLength(30)]
        [DisplayName("Payment details")]
        public string Details { get; set; }
    }
}
