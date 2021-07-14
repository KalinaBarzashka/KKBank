using System;
using System.ComponentModel;

namespace KKBank.Web.ViewModels.ViewModels.Transactions
{
    public class TransactionViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Details { get; set; }

        public bool IsIncome { get; set; }

        [DisplayName("Payer/Payee")]
        public string PayerPayee { get; set; }

        //[DisplayName("Payer/Payee Account")]
        //public string PayerPayeeAccount { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

    }
}
