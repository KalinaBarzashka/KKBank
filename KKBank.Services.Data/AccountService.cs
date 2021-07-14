using KKBank.Data;
using KKBank.Data.Models;
using KKBank.Web.ViewModels.ViewModels.Account;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKBank.Services.Data
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext dbContext;

        public AccountService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllActiveAccountTypesAsKeyValuePairs()
        {
            return this.dbContext.AccountTypes.Where(x => x.IsDeleted_17118069 == false).Select(x => new
            {
                x.Id,
                x.AccountTypeName
            }).ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.AccountTypeName));
        }

        public AccountsViewModel GetActiveAccountsForUser(string userId, int pageId, int itemsPerPage = 5)
        {
            var accounts = dbContext.Accounts.Where(x => x.UserId == userId && x.IsDeleted_17118069 == false)
                .OrderBy(x => x.Id)
                .Skip((pageId - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(x => new AccountViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    AccountTypeName = x.AccountType.AccountTypeName,
                    Available = x.Available,
                    BlockedАmount = x.BlockedАmount,
                    IBAN = x.IBAN,
                    CurrencyAbbreviation = x.Currency.CurrencyAbbreviation
                }).ToList();

            var viewModel = new AccountsViewModel
            {
                Accounts = accounts,
                PageNumber = pageId,
                TotalItemsCount = ActiveAccountsCountForUser(userId),
                ItemsPerPage = itemsPerPage
            };

            return viewModel;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllActiveAccountsZeroMoneyForUser(string userId)
        {
            return this.dbContext.Accounts.Where(x => x.UserId == userId && x.Available == 0 && x.BlockedАmount == 0 && x.IsDeleted_17118069 == false).Select(x => new
            {
                Id = x.Id,
                Account = "Name: " + x.Name + ", Currency: " + x.Currency.CurrencyAbbreviation
            }).ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Account));
        }

        public int GetCheckingAccountId()
        {
            return this.dbContext.AccountTypes.Where(x => x.AccountTypeName == "Checking Account").Select(x => x.Id).FirstOrDefault();
        }

        private int ActiveAccountsCountForUser(string userId)
        {
            return this.dbContext.Accounts.AsNoTracking().Where(x => x.UserId == userId && x.IsDeleted_17118069 == false).Count();
        }

        public IEnumerable<AccountViewModel> GetActiveAccountsForUser(string userId)
        {
            var accounts = dbContext.Accounts.Where(x => x.UserId == userId && x.IsDeleted_17118069 == false)
                .OrderBy(x => x.Id)
                .Select(x => new AccountViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    AccountTypeName = x.AccountType.AccountTypeName,
                    Available = x.Available,
                    BlockedАmount = x.BlockedАmount,
                    IBAN = x.IBAN,
                    CurrencyAbbreviation = x.Currency.CurrencyAbbreviation
                }).ToList();

            return accounts;
        }

        public IEnumerable<AccountViewModel> GetActiveAccountsForUserForExport(string userId)
        {
            var accounts = dbContext.Accounts.Where(x => x.UserId == userId && x.IsDeleted_17118069 == false)
                .OrderBy(x => x.Id)
                .Select(x => new AccountViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    AccountTypeName = x.AccountType.AccountTypeName,
                    Available = x.Available,
                    BlockedАmount = x.BlockedАmount,
                    IBAN = x.IBAN,
                    CurrencyAbbreviation = x.Currency.CurrencyAbbreviation
                }).ToList();

            return accounts;
        }

        public Account GetAccountByUserAndAccountId(string userId, int accountId)
        {
            return dbContext.Accounts.Where(x => x.UserId == userId && x.Id == accountId).FirstOrDefault();
        }

        public Account GetAccountByIban(string iban)
        {
            return dbContext.Accounts.Where(x => x.IBAN == iban).FirstOrDefault();
        }

        public int GetOtherUsersAccountIdByIban(string IBAN, string userId, int transferCurrencyId)
        {
            return this.dbContext.Accounts
                .Where(x => x.UserId != userId && x.CurrencyId == transferCurrencyId && x.IBAN == IBAN)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public IEnumerable<KeyValuePair<int, string>> GetAllActiveAccountsForUserFromTransactionFilter(string userId)
        {
            return this.dbContext.Accounts.Where(x => x.UserId == userId && x.IsDeleted_17118069 == false)
                .Select(x => new
                {
                    Id = x.Id,
                    AccountName = x.Name + " - " + x.Currency.CurrencyAbbreviation + " - " + x.IBAN
                }).ToList().Select(x => new KeyValuePair<int, string>(x.Id, x.AccountName)).ToList();
        }

        public AccountViewModel GetAccountById(int id)
        {
            return this.dbContext.Accounts
                .Where(x => x.Id == id)
                .Select(x => new AccountViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    AccountTypeName = x.AccountType.AccountTypeName,
                    Available = x.Available,
                    BlockedАmount = x.BlockedАmount,
                    IBAN = x.IBAN,
                    CurrencyAbbreviation = x.Currency.CurrencyAbbreviation
                }).FirstOrDefault();
        }

        public async Task<bool> AddMoneyToAccount(int accountId, string amount)
        {
            var account = this.dbContext.Accounts.Where(x => x.Id == accountId).FirstOrDefault();

            if(decimal.TryParse(amount, out decimal parsedAmount))
            {
                account.Available += parsedAmount;
                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
