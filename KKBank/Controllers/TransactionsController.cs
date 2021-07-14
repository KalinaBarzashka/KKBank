using ClosedXML.Excel;
using KKBank.Data.Models;
using KKBank.Services.Data;
using KKBank.Web.ViewModels.ViewModels.Account;
using KKBank.Web.ViewModels.ViewModels.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace KKBank.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class TransactionsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITransactionsService transactionsService;
        private readonly IAccountService accountService;

        public TransactionsController(UserManager<ApplicationUser> userManager, 
            ITransactionsService transactionsService,
            IAccountService accountService)
        {
            this.userManager = userManager;
            this.transactionsService = transactionsService;
            this.accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            string userId = user.Id;

            var viewModel = new TransactionsViewModel();

            AddTransactionTypesAccountsAndTransactions(userId, viewModel, 0);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(TransactionsViewModel input)
        {
            string userId = (await this.userManager.GetUserAsync(this.User)).Id;
            int accountId = input.AccountsId;
            string details = input.Details;
            string filterType = input.FilerType;
            string lastNRequests = input.LastNRequests;
            string toDate = input.ToDate;
            int transactionTypeId = input.TransactionTypeId;
        
            var isAccountValid = this.transactionsService.ValidateAccountId(userId, accountId);
            if (!this.ModelState.IsValid || !isAccountValid)
            {
                AddTransactionTypesAccountsAndTransactions(userId, input, 0);
                return this.View(input);
            }
        
            var transactions = this.transactionsService.GetFilteredTransactionsForUser(userId, filterType, lastNRequests, toDate, accountId, details,transactionTypeId);
        
            AddTransactionTypesAndAccounts(userId, input);
            input.Transactions = transactions;
        
            return this.View(input);
        }

        private void AddTransactionTypesAccountsAndTransactions(string userId, TransactionsViewModel viewModel, int searchedAccountId)
        {
            int accountId = AddTransactionTypesAndAccounts(userId, viewModel);
            viewModel.AccountsId = accountId;

            if (searchedAccountId != 0)
            {
                accountId = searchedAccountId;
            }
            viewModel.Transactions = this.transactionsService.GetLastTenTransactionsForUserAccount(accountId);
        }

        private int AddTransactionTypesAndAccounts(string userId, TransactionsViewModel viewModel)
        {
            var transactionTypes = new List<KeyValuePair<string, string>>();
            transactionTypes.Add(new KeyValuePair<string, string>("0", "All"));
            transactionTypes.Add(new KeyValuePair<string, string>("1", "Incomes"));
            transactionTypes.Add(new KeyValuePair<string, string>("2", "Transfers and payments"));
            var userAccounts = this.accountService.GetAllActiveAccountsForUserFromTransactionFilter(userId);

            viewModel.TransactionTypes = transactionTypes;
            viewModel.Accounts = userAccounts;

            if(userAccounts.Count() == 0)
            {
                return 0;
            }

            return userAccounts.ToList()[0].Key;
        }

        [HttpPost]
        public IActionResult Export(List<int> transactions, int accountId)
        {
            var transactionsData = this.transactionsService.GetTransactionsByIds(transactions, accountId);
            var accountData = this.accountService.GetAccountById(accountId);
            return this.Export(transactionsData, accountData);
        }

        private IActionResult Export(IEnumerable<TransactionViewModel> transactions, AccountViewModel accountData)
        {
            string fileName = "transactions.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Transactions report");
                worksheet.Cell(1, 1).Value = "Account Name";
                worksheet.Cell(1, 2).Value = "Currency";
                worksheet.Cell(1, 3).Value = "IBAN";
                worksheet.Cell(1, 4).Value = "Available";

                worksheet.Cell(2, 1).Value = accountData.Name;
                worksheet.Cell(2, 2).Value = accountData.CurrencyAbbreviation;
                worksheet.Cell(2, 3).Value = accountData.IBAN;
                worksheet.Cell(2, 4).Value = accountData.Available;

                worksheet.Cell(4, 1).Value = "Created On";
                worksheet.Cell(4, 2).Value = "Details";
                worksheet.Cell(4, 3).Value = "Payer / Payee";
                worksheet.Cell(4, 4).Value = "Debit";
                worksheet.Cell(4, 5).Value = "Credit";
        
                for (int index = 1; index <= 5; index++)
                {
                    worksheet.Cell(4, index).Style.Font.Bold = true;
                }

                var listTransactions = transactions.ToList();

                for (int index = 1; index <= transactions.Count(); index++)
                {
                    worksheet.Cell(index + 4, 1).Value = listTransactions[index - 1].CreatedOn;
                    worksheet.Cell(index + 4, 2).Value = listTransactions[index - 1].Details;
                    worksheet.Cell(index + 4, 3).Value = listTransactions[index - 1].PayerPayee;
                    if (listTransactions[index - 1].IsIncome == true)
                    {
                        worksheet.Cell(index + 4, 5).Value = listTransactions[index - 1].Credit;
                        worksheet.Cell(index + 4, 5).Style.NumberFormat.NumberFormatId = 2;
                    }
                    else
                    {
                        worksheet.Cell(index + 4, 4).Value = listTransactions[index - 1].Debit;
                        worksheet.Cell(index + 4, 4).Style.NumberFormat.NumberFormatId = 2;
                    }
                }
        
                IXLRange range = worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(2, 4));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                range.Style.Border.InsideBorder = XLBorderStyleValues.Dashed;
                //range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(0xD4C1D9));

                IXLRange rangeData = worksheet.Range(worksheet.Cell(4, 1), worksheet.Cell(transactions.Count() + 4, 5));
                rangeData.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                rangeData.Style.Border.InsideBorder = XLBorderStyleValues.Dashed;
                rangeData.Style.Fill.SetBackgroundColor(XLColor.FromArgb(0xD4C1D9));

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
        
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
    }
}
