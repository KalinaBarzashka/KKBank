using System;

namespace KKBank.Web.ViewModels.ViewModels.Request
{
    public class BoRequestViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string AccountName { get; set; }

        public string AccountTypeName { get; set; }

        public string CurrencyName { get; set; }

        public string Description { get; set; }

        public string StatusName { get; set; }

        public string RequestTypeName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SignedFromBankEmployeeName { get; set; }

        public string ModifiedOn { get; set; }
    }
}
