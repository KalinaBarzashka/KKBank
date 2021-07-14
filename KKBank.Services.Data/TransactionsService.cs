using KKBank.Data;
using KKBank.Web.ViewModels.ViewModels.Transactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KKBank.Services.Data
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAccountService accountService;

        public TransactionsService(ApplicationDbContext dbContext,
            IAccountService accountService)
        {
            this.dbContext = dbContext;
            this.accountService = accountService;
        }

        public IEnumerable<TransactionViewModel> GetLastTenTransactionsForUserAccount(int accountId)
        {
            return this.dbContext.PaymentOrders
                .Include(x => x.Payor)
                .Include(x => x.Payee)
                .Where(x => x.FromAccountId == accountId || x.ToAccountId == accountId)
                .OrderByDescending(x => x.CreatedOn_17118069)
                .Take(10)
                .Select(x => new TransactionViewModel
                {
                    Id= x.Id,
                    CreatedOn = x.CreatedOn_17118069,
                    Details = x.DetailsOfPayment,
                    IsIncome = x.ToAccountId == accountId ? true : false,
                    PayerPayee = x.ToAccountId == accountId ? x.Payor.FirstName + " " + x.Payor.MiddleName + " " + x.Payor.LastName : x.Payee.FirstName + " " + x.Payee.MiddleName + " " + x.Payee.LastName,
                    Debit = x.ToAccountId == accountId ? 0M : x.FromAmount,
                    Credit = x.ToAccountId == accountId ? x.ToAmount : 0M
                })
                .ToList();
        }

        public IEnumerable<TransactionViewModel> GetFilteredTransactionsForUser(string userId, string filterType, string lastNRequests, string toDate, int accountId, string details, int transactionTypeId)
        {
            var lastNQuery = String.Empty;
            var dateQuery = String.Empty;
            var offset = String.Empty;
            var accountIdQuery = String.Empty;
            var detailsQuery = String.Empty;
            var transactionTypeIdQuery = String.Empty;
            

            if (filterType == "1")
            {
                lastNQuery = $" TOP({lastNRequests})";
            }
            else if (filterType == "2")
            {
                var date = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                dateQuery = $" and CAST([po].CreatedOn_17118069 as date) = '{date}'";
                offset = " OFFSET 0 ROWS";
            }

            if (transactionTypeId == 0)
            {
                transactionTypeIdQuery = $"(([po].[FromAccountId] = {accountId}) OR ([po].[ToAccountId] = {accountId}))";
            } 
            else if (transactionTypeId == 1)
            {
                transactionTypeIdQuery = $"[po].[ToAccountId] = {accountId}";
            }
            else if (transactionTypeId == 2)
            {
                transactionTypeIdQuery = $"[po].[FromAccountId] = {accountId}";
            }

            if (details != null)
            {
                detailsQuery = $" and [po].[DetailsOfPayment] = '{details}'";
            }

            string query = @$"
                SELECT{lastNQuery} *
                FROM [KKBank].[17118069].[PaymentOrders] po
                WHERE {transactionTypeIdQuery}{dateQuery}{detailsQuery}
                ORDER BY po.CreatedOn_17118069 DESC{offset}
                ";

            var viewModel = dbContext.PaymentOrders.FromSqlRaw(query)
                .AsNoTracking()
                .Select(x => new TransactionViewModel
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn_17118069,
                    Details = x.DetailsOfPayment,
                    IsIncome = x.ToAccountId == accountId ? true : false,
                    PayerPayee = x.ToAccountId == accountId ? x.Payor.FirstName + " " + x.Payor.MiddleName + " " + x.Payor.LastName : x.Payee.FirstName + " " + x.Payee.MiddleName + " " + x.Payee.LastName,
                    Debit = x.ToAccountId == accountId ? 0M : x.FromAmount,
                    Credit = x.ToAccountId == accountId ? x.ToAmount : 0M
                }).ToList();

            return viewModel;
        }

        public bool ValidateAccountId(string userId, int accountId)
        {
            var accountIds = this.accountService.GetAllActiveAccountsForUserFromTransactionFilter(userId).Select(x => x.Key).ToList();

            if (!accountIds.Contains(accountId))
            {
                return false;
            }
            return true;
        }

        public IEnumerable<TransactionViewModel> GetTransactionsByIds(List<int> transactions, int accountId)
        {
            return this.dbContext.PaymentOrders
                .Where(x => transactions.Contains(x.Id))
                .Select(x => new TransactionViewModel
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn_17118069,
                    Details = x.DetailsOfPayment,
                    IsIncome = x.ToAccountId == accountId ? true : false,
                    PayerPayee = x.ToAccountId == accountId ? x.Payor.FirstName + " " + x.Payor.MiddleName + " " + x.Payor.LastName : x.Payee.FirstName + " " + x.Payee.MiddleName + " " + x.Payee.LastName,
                    Debit = x.ToAccountId == accountId ? 0M : x.FromAmount,
                    Credit = x.ToAccountId == accountId ? x.ToAmount : 0M
                })
                .ToList();
        }
    }
}
