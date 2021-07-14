using KKBank.Data;
using KKBank.Data.Models;
using KKBank.Web.ViewModels.ViewModels.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKBank.Services.Data
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IAccountService accountService;
        private readonly ICurrencyService currencyService;
        private readonly ApplicationDbContext dbContext;
        private readonly IStatusService statusService;

        public PaymentsService(IAccountService accountService,
            ICurrencyService currencyService,
            ApplicationDbContext dbContext,
            IStatusService statusService)
        {
            this.accountService = accountService;
            this.currencyService = currencyService;
            this.dbContext = dbContext;
            this.statusService = statusService;
        }

        public PaymentsBetweenOwnAccountsViewModel FillBetweenOwnAccountsViewModel(string userId)
        {
            var accounts = this.accountService.GetActiveAccountsForUser(userId);

            var viewModel = new PaymentsBetweenOwnAccountsViewModel
            {
                FromAccounts = accounts.Select(x => 
                new KeyValuePair<string, string>(x.Id.ToString(), (x.Name + $" ({x.Available} {x.CurrencyAbbreviation})"))).ToList(),
                ToAccounts = accounts.Select(x => 
                new KeyValuePair<string, string>(x.Id.ToString(), (x.Name + $" ({x.Available} {x.CurrencyAbbreviation})"))).ToList(),
            };

            return viewModel;
        }

        public bool ValidateInputBetweenOwnAccountsPayment(int accountFromId, int accountToId, string amount, string userId, string description)
        {
            var userAccounts = this.accountService.GetActiveAccountsForUser(userId);
            var accountIds = userAccounts.Select(x => x.Id).ToList();
            bool isValidAmount = decimal.TryParse(amount, out _);

            if (!accountIds.Contains(accountFromId) 
                || !accountIds.Contains(accountToId) 
                || !isValidAmount 
                || description.Length == 0
                || description.Length> 30) //check amount length
            {
                return false;
            }

            decimal fromAccountAvailable = userAccounts.Where(x => x.Id == accountFromId).Select(x => x.Available).FirstOrDefault();
            if (fromAccountAvailable < decimal.Parse(amount))
            {
                return false;
            }

            return true;
        }

        public async Task ExecuteBetweenOwnAccountsPayment(int accountFromId, int accountToId, decimal fromAmount, string userId, string description)
        {
            Account accountFrom = this.accountService.GetAccountByUserAndAccountId(userId, accountFromId);
            Account accountTo = this.accountService.GetAccountByUserAndAccountId(userId, accountToId);

            int fromCurrencyId = accountFrom.CurrencyId;
            int toCurrencyId = accountTo.CurrencyId;

            string fromCurrencyAbbriv = this.currencyService.GetCurrencyAbbrivByCurrencyId(fromCurrencyId);
            string toCurrencyAbbriv = this.currencyService.GetCurrencyAbbrivByCurrencyId(toCurrencyId);

            decimal toAmount = CurrencyConverter(fromAmount, fromCurrencyAbbriv, toCurrencyAbbriv);

            var payment = new PaymentOrder()
            {
                FromAccountId = accountFrom.Id,
                ToAccountId = accountTo.Id,
                FromAmount = fromAmount,
                ToAmount = toAmount,
                CurrencyFromId = fromCurrencyId,
                CurrencyToId = toCurrencyId,
                TransferCurrencyId = toCurrencyId,
                DetailsOfPayment = description,
                PaymentOrderStatusId = GetOperationExecutedPaymentStatusId(),
                PayorId = userId,
                PayeeId = userId,
                CreatedOn_17118069 = DateTime.UtcNow
            };

            dbContext.PaymentOrders.Add(payment);
            accountFrom.Available -= fromAmount;
            accountTo.Available += toAmount;

            await dbContext.SaveChangesAsync();
        }

        public bool ValidateInputToKKBankAccountPayment(string userId, int accountFromId, string IBAN, string beneficiaryName, int transferCurrencyId, string amountFrom, string amountTo, string paymentDetails)
        {
                //проверка IBAN съществува ли?
                //IBAN-a на друг клиент ли е?
                //съществуващия IBAN съвпада ли с валутата, посочена от клиента?
            //посочената ОТ валута съвпада ли с валутата на ОТ сметката?
                //съвпада ли конвертираната сума при клиента с конвертираната при нас?
            //провери име на бенефициент
                //сметката ОТ има ли достатъчна наличност?
                //сметката от на текущия потребител ли е?
                //проверка за IBAN?
                //провери детайли на плащането?



            var userAccounts = this.accountService.GetActiveAccountsForUser(userId).ToList();
            var accountIds = userAccounts.Select(x => x.Id).ToList();

            int accountToId = this.accountService.GetOtherUsersAccountIdByIban(IBAN, userId, transferCurrencyId);

            bool isValidAmountFrom = decimal.TryParse(amountFrom, out _);
            bool isValidAmountTo = decimal.TryParse(amountTo, out _);

            if (!accountIds.Contains(accountFromId)
                || !isValidAmountFrom
                || !isValidAmountTo
                || paymentDetails.Length == 0
                || paymentDetails.Length > 30
                || accountToId == 0)
            {
                return false;
            }
            
            decimal fromAccountAvailable = userAccounts.Where(x => x.Id == accountFromId).Select(x => x.Available).FirstOrDefault();
            if (fromAccountAvailable < decimal.Parse(amountFrom))
            {
                return false;
            }

            string currFrom = userAccounts.Where(x => x.Id == accountFromId).Select(x => x.CurrencyAbbreviation).FirstOrDefault();
            string currTo = this.currencyService.GetCurrencyAbbrivByCurrencyId(transferCurrencyId);

            decimal convertedAmountTo = this.CurrencyConverter(decimal.Parse(amountFrom), currFrom, currTo);

            if (decimal.Parse(amountTo) != Math.Round(convertedAmountTo, 2))
            {
                return false;
            }

            return true;
        }

        public async Task ExecuteToKKBankAccountPayment(string userId, int accountFromId, string IBAN, string beneficiaryName, int transferCurrencyId, decimal amountFrom, decimal amountTo, string paymentDetails, string additionalDetails)
        {
            Account accountFrom = this.accountService.GetAccountByUserAndAccountId(userId, accountFromId);
            Account accountTo = this.accountService.GetAccountByIban(IBAN);

            int fromCurrencyId = accountFrom.CurrencyId;
            int toCurrencyId = accountTo.CurrencyId;

            string fromCurrencyAbbriv = this.currencyService.GetCurrencyAbbrivByCurrencyId(fromCurrencyId);
            string toCurrencyAbbriv = this.currencyService.GetCurrencyAbbrivByCurrencyId(toCurrencyId);

            //decimal convertedAmountTo = this.CurrencyConverter(amountFrom, fromCurrencyAbbriv, toCurrencyAbbriv);

            var payment = new PaymentOrder()
            {
                FromAccountId = accountFrom.Id,
                ToAccountId = accountTo.Id,
                FromAmount = amountFrom,
                ToAmount = amountTo,
                CurrencyFromId = fromCurrencyId,
                CurrencyToId = toCurrencyId,
                TransferCurrencyId = toCurrencyId,
                DetailsOfPayment = paymentDetails,
                PaymentOrderStatusId = GetOperationExecutedPaymentStatusId(),
                BeneficiaryName = beneficiaryName,
                IBAN = IBAN,
                AdditionalDetails = additionalDetails,
                PayorId = userId,
                PayeeId = accountTo.UserId,
                CreatedOn_17118069 = DateTime.UtcNow
            };

            dbContext.PaymentOrders.Add(payment);
            accountFrom.Available -= amountFrom;
            accountTo.Available += amountTo;

            await dbContext.SaveChangesAsync();
        }

        private int GetOperationExecutedPaymentStatusId()
        {
            return this.dbContext.PaymentOrderStatus.Where(x => x.StatusName == "Operation executed").Select(x => x.Id).FirstOrDefault();
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

        public PaymentsToKKBankAccountViewModel FillToKKBankAccountViewModel(string userId)
        {
            var accounts = this.accountService.GetActiveAccountsForUser(userId);

            var viewModel = new PaymentsToKKBankAccountViewModel
            {
                FromAccounts = accounts.Select(x =>
                new KeyValuePair<string, string>(x.Id.ToString(), (x.Name + $" ({x.Available} {x.CurrencyAbbreviation})"))).ToList(),
                Currency = this.currencyService.GetAllActiveCurrencyAsKeyValuePairs()
            };

            return viewModel;
        }

        public PaymentsViewModel GetAllPaymentsForUser(string userId)
        {
            var payments = this.dbContext.PaymentOrders
                .Include(p => p.Payor)
                .Include(p => p.Payee)
                .Include(x => x.PaymentOrderStatus)
                .Include(x => x.CurrencyFrom)
                .Include(x => x.FromAccount)
                .Include(x => x.ToAccount)
                .AsNoTracking()
                .Where(x => x.PayorId == userId)
                .OrderByDescending(x => x.Id)
                .Take(10)
                .Select(x => new PaymentViewModel
                {
                    Id = x.Id,
                    PayorId = x.PayorId,
                    PayeeId = x.PayeeId,
                    PayorInfo = x.Payor.FirstName + " " + x.Payor.MiddleName + " " + x.Payor.LastName + ", " + x.FromAccount.IBAN,
                    PayeeInfo = x.Payee.FirstName + " " + x.Payee.MiddleName + " " + x.Payee.LastName + ", " + x.ToAccount.IBAN,
                    CurrencyFromAbbriv = x.CurrencyFrom.CurrencyAbbreviation,
                    FromAmount = x.FromAmount,
                    CreatedOn = x.CreatedOn_17118069.ToLocalTime(),
                    PaymentOrderStatus = x.PaymentOrderStatus.StatusName,
                    PaymentType = x.IBAN == null ? "Between My Accounts" : "To KK Bank Account"
                })
                .ToList();

            var viewModel = new PaymentsViewModel
            {
                Payments = payments,
                StatusItems = this.statusService.GetAllActivePaymentOrderStatusAsKeyValuePairs()
            };

            return viewModel;
        }

        public IEnumerable<PaymentViewModel> GetFilteredPaymentsForUser(string userId, string filterType, string lastNRequests, string toDate, int statusId)
        {
            var lastNQuery = String.Empty;
            var dateQuery = String.Empty;
            var statusQuery = String.Empty;
            var offset = String.Empty;

            if (filterType == "1")
            {
                lastNQuery = $" TOP({lastNRequests})";
            }
            else if (filterType == "2")
            {
                var date = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                dateQuery = $" and CAST(po.CreatedOn_17118069 as date) = '{date}'";
                offset = " OFFSET 0 ROWS";
            }

            if (statusId > 0)
            {
                statusQuery = $" and po.PaymentOrderStatusId = '{statusId}'";
            }

            string query = @$"
                SELECT{lastNQuery} *
                FROM [KKBank].[17118069].[PaymentOrders] po
                WHERE po.PayorId = '{userId}'{dateQuery}{statusQuery}
                ORDER BY po.CreatedOn_17118069 DESC{offset}
                ";

            var viewModel = dbContext.PaymentOrders.FromSqlRaw(query)
                .AsNoTracking()
                .Select(x => new PaymentViewModel
                {
                    Id = x.Id,
                    PayorId = x.PayorId,
                    PayeeId = x.PayeeId,
                    PayorInfo = x.Payor.FirstName + " " + x.Payor.MiddleName + " " + x.Payor.LastName + ", " + x.FromAccount.IBAN,
                    PayeeInfo = x.Payee.FirstName + " " + x.Payee.MiddleName + " " + x.Payee.LastName + ", " + x.ToAccount.IBAN,
                    CurrencyFromAbbriv = x.CurrencyFrom.CurrencyAbbreviation,
                    FromAmount = x.FromAmount,
                    CreatedOn = x.CreatedOn_17118069.ToLocalTime(),
                    PaymentOrderStatus = x.PaymentOrderStatus.StatusName,
                    PaymentType = x.IBAN == null ? "Between My Accounts" : "To KK Bank Account"
                }).ToList();

            return viewModel;
        }

        public IEnumerable<PaymentViewModel> GetPaymentsByIds(List<int> payments)
        {
            return this.dbContext.PaymentOrders
                .Where(x => payments.Contains(x.Id))
                .Select(x => new PaymentViewModel
                {
                    Id = x.Id,
                    PayorId = x.PayorId,
                    PayeeId = x.PayeeId,
                    PayorInfo = x.Payor.FirstName + " " + x.Payor.MiddleName + " " + x.Payor.LastName + ", " + x.FromAccount.IBAN,
                    PayeeInfo = x.Payee.FirstName + " " + x.Payee.MiddleName + " " + x.Payee.LastName + ", " + x.ToAccount.IBAN,
                    CurrencyFromAbbriv = x.CurrencyFrom.CurrencyAbbreviation,
                    FromAmount = x.FromAmount,
                    CreatedOn = x.CreatedOn_17118069.ToLocalTime(),
                    PaymentOrderStatus = x.PaymentOrderStatus.StatusName,
                    PaymentType = x.IBAN == null ? "Between My Accounts" : "To KK Bank Account"
                })
                .ToList();
        }
    }
}
