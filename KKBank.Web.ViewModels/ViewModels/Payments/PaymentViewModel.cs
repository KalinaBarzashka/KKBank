using System;

namespace KKBank.Web.ViewModels.ViewModels.Payments
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        public string PayorId { get; set; }

        public string PayeeId { get; set; }

        public string PayorInfo { get; set; }

        public string PayeeInfo { get; set; }

        public string CurrencyFromAbbriv { get; set; }

        public decimal FromAmount { get; set; }

        public DateTime CreatedOn { get; set; }

        public string PaymentOrderStatus { get; set; }

        public string PaymentType { get; set; }
    }
}
