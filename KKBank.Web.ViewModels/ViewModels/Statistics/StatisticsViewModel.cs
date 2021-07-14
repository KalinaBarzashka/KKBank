namespace KKBank.Web.ViewModels.ViewModels.Statistics
{
    public class StatisticsViewModel
    {
        public int TotalBankUsers { get; set; }

        public int TotalUserAccounts { get; set; }

        public int TotalTransactions { get; set; }

        public int TotalCurrencies { get; set; }

        public int TotalRequestsByUsers { get; set; }

        public int TotalHandledRequests { get; set; }

        public decimal TotalMoney { get; set; }
    }
}
