using KKBank.Data;
using KKBank.Web.ViewModels.ViewModels.Statistics;
using System;
using System.Linq;

namespace KKBank.Services.Data
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IStatusService statusService;

        public StatisticsService(ApplicationDbContext dbContext,
            IStatusService statusService)
        {
            this.dbContext = dbContext;
            this.statusService = statusService;
        }

        public void GetStatistics(StatisticsViewModel viewModel)
        {
            viewModel.TotalBankUsers = this.dbContext.ApplicationUsers
                .Where(x => x.IsDeleted == false)
                .Count();

            viewModel.TotalUserAccounts = this.dbContext.Accounts
                .Where(x => x.IsDeleted_17118069 == false)
                .Count();

            int operationExecutedStatus = this.statusService.GetOperationExecutedStatusId();

            viewModel.TotalTransactions = this.dbContext.PaymentOrders
                .Where(x => x.IsDeleted_17118069 == false && x.PaymentOrderStatusId == operationExecutedStatus)
                .Count();

            viewModel.TotalCurrencies = this.dbContext.Currency
                .Where(x => x.IsDeleted_17118069 == false)
                .Count();

            viewModel.TotalRequestsByUsers = this.dbContext.AccountRequests
                .Where(x => x.IsDeleted_17118069 == false)
                .Count();

            var awaitingAproval = this.statusService.GetAwaitingApprovalStatusId();

            viewModel.TotalHandledRequests = this.dbContext.AccountRequests
                .Where(x => x.IsDeleted_17118069 == false && x.StatusId != awaitingAproval)
                .Count();

            var accounts = this.dbContext.Accounts.Select(x => new
            {
                Amount = x.Available,
                Currency = x.Currency.CurrencyAbbreviation
            });

            var totalMoney = 0M;
            foreach (var account in accounts)
            {
                totalMoney += this.CurrencyConverter(account.Amount, account.Currency, "EUR");
            }

            viewModel.TotalMoney = Math.Round(totalMoney, 2);
        }

        private decimal CurrencyConverter(decimal fromAmount, string fromCurrency, string toCurrency)
        {
            decimal toAmount = 0M;

            if (fromCurrency == "BGN")
            {
                if (toCurrency == "BGN")
                {
                    toAmount = fromAmount * 1.0000000M;
                }
                else if (toCurrency == "EUR")
                {
                    toAmount = fromAmount / 1.9600000M;
                }
                else if (toCurrency == "USD")
                {
                    toAmount = fromAmount / 1.6420000M;
                }
            }
            else if (fromCurrency == "EUR")
            {
                if (toCurrency == "EUR")
                {
                    toAmount = fromAmount * 1.0000000M;
                }
                else if (toCurrency == "BGN")
                {
                    toAmount = fromAmount * 1.9510000M;
                }
                else if (toCurrency == "USD")
                {
                    toAmount = fromAmount * 1.1880000M;
                }
            }
            else if (fromCurrency == "USD")
            {
                if (toCurrency == "USD")
                {
                    toAmount = fromAmount * 1.0000000M;
                }
                else if (toCurrency == "BGN")
                {
                    toAmount = fromAmount * 1.5819000M;
                }
                else if (toCurrency == "EUR")
                {
                    toAmount = fromAmount * 0.8060000M;
                }
            }

            return toAmount;
        }
    }
}
