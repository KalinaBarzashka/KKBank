using KKBank.Web.ViewModels.ViewModels.Transactions;
using System.Collections.Generic;

namespace KKBank.Services.Data
{
    public interface ITransactionsService
    {
        public IEnumerable<TransactionViewModel> GetLastTenTransactionsForUserAccount(int accountId);

        public IEnumerable<TransactionViewModel> GetFilteredTransactionsForUser(string userId, string filterType, string lastNRequests, string toDate, int accountId, string details, int transactionTypeId);

        public bool ValidateAccountId(string userId, int accountId);

        public IEnumerable<TransactionViewModel> GetTransactionsByIds(List<int> transactions, int accountId);
    }
}
