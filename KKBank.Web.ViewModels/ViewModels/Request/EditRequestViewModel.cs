using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Web.ViewModels.ViewModels.Request
{
    public class EditRequestViewModel
    {
        public EditRequestViewModel()
        {
            this.CurrencyItems = new List<KeyValuePair<string, string>>();
        }

        public int Id { get; set; } //request id

        [Display(Name = "Account name")]
        public string AccountName { get; set; }

        public int? AccountId { get; set; }

        public string AccountTypeName { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        [Display(Name = "Reason for Deletion")]
        public string Description { get; set; }

        public string RequestTypeName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public string StatusName { get; set; }

        public bool IsDelete { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CurrencyItems { get; set; }
    }
}
