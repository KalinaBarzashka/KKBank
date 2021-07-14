using KKBank.Data.Models;
using KKBank.Web.ViewModels.ViewModels.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KKBank.Services.Data
{
    public interface IAccountService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllActiveAccountTypesAsKeyValuePairs();

        public AccountsViewModel GetActiveAccountsForUser(string userId, int pageId, int itemsPerPage = 5);

        public IEnumerable<KeyValuePair<string, string>> GetAllActiveAccountsZeroMoneyForUser(string userId);

        public int GetCheckingAccountId();

        public IEnumerable<AccountViewModel> GetActiveAccountsForUser(string userId);

        public IEnumerable<AccountViewModel> GetActiveAccountsForUserForExport(string userId);

        public Account GetAccountByUserAndAccountId(string userId, int account);

        public Account GetAccountByIban(string iban);

        public int GetOtherUsersAccountIdByIban(string IBAN, string userId, int transferCurrencyId);

        public IEnumerable<KeyValuePair<int, string>> GetAllActiveAccountsForUserFromTransactionFilter(string userId);

        public AccountViewModel GetAccountById(int id);

        public Task<bool> AddMoneyToAccount(int accountId, string amount);
    }
}
