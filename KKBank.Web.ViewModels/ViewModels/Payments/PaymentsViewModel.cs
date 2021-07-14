using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Web.ViewModels.ViewModels.Payments
{
    public class PaymentsViewModel
    {
        public PaymentsViewModel()
        {
            this.Payments = new List<PaymentViewModel>();
            this.StatusItems = new List<KeyValuePair<string, string>>();
        }

        public IEnumerable<PaymentViewModel> Payments { get; set; }

        public int StatusId { get; set; }
        public IEnumerable<KeyValuePair<string, string>> StatusItems { get; set; }

        public string FilerType { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "The field must contain only digits!")]
        [RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Please enter a number!")]
        [MaxLength(3)]
        public string LastNRequests { get; set; }

        public string ToDate { get; set; }
    }
}
