using ClosedXML.Excel;
using KKBank.Data;
using KKBank.Data.Models;
using KKBank.Services.Data;
using KKBank.Web.ViewModels.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KKBank.Controllers
{
    
    [Authorize(Roles = "User")]
    public class AccountController : Controller
    {
        private const int itemsPerPage = 5;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAccountService accountService;
        private readonly ICurrencyService currencyService;
        private readonly IRequestService requestService;
        private readonly IStatusService statusService;

        public AccountController(UserManager<ApplicationUser> userManager, 
            IAccountService accountService, 
            ICurrencyService currencyService,
            IRequestService requestService,
            IStatusService statusService)
        {
            this.userManager = userManager;
            this.accountService = accountService;
            this.currencyService = currencyService;
            this.requestService = requestService;
            this.statusService = statusService;
        }

        // GET: AccountController
        public async Task<IActionResult> All(int id = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = accountService.GetActiveAccountsForUser(user.Id, id, itemsPerPage);
            ViewData["UserId"] = user.Id;

            return View(viewModel);
        }

        // GET: AccountController/Create
        public IActionResult Create()
        {
            var viewModel = new CreateAccountInputModel();
            viewModel.CurrencyItems = this.currencyService.GetAllActiveCurrencyAsKeyValuePairs();
            viewModel.AccountTypesItems = this.accountService.GetAllActiveAccountTypesAsKeyValuePairs();

            return this.View(viewModel);
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAccountInputModel input) //IFormCollection collection
        {
            if (!this.ModelState.IsValid)
            {
                input.CurrencyItems = this.currencyService.GetAllActiveCurrencyAsKeyValuePairs();
                input.AccountTypesItems = this.accountService.GetAllActiveAccountTypesAsKeyValuePairs();
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.requestService.CreateAddAccountRequestAsync(input, user.Id);
            return this.Redirect("/Request/Archive");
        }

        // GET: AccountController/Delete
        public async Task<IActionResult> Delete()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = new DeleteAccountInputModel();
            viewModel.UserAccounts = this.accountService.GetAllActiveAccountsZeroMoneyForUser(user.Id);
            return View(viewModel);
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteAccountInputModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                input.UserAccounts = this.accountService.GetAllActiveAccountsZeroMoneyForUser(user.Id);
                return this.View(input);
            }

            await this.requestService.CreateDeleteAccountRequestAsync(input, user.Id);
            return this.Redirect("/Request/Archive");
        }

        [HttpGet]
        public async Task<IActionResult> ExportAll() 
        {
            var currentUserId = (await this.userManager.GetUserAsync(this.User)).Id;
            var accounts = accountService.GetActiveAccountsForUserForExport(currentUserId).ToList();

            return this.Export(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> ExportCurrentPage(int id)
        {
            var currentUserId = (await this.userManager.GetUserAsync(this.User)).Id;
            var accountsModel = accountService.GetActiveAccountsForUser(currentUserId, id, itemsPerPage);

            var accounts = accountsModel.Accounts.ToList();
            return this.Export(accounts);
        }

    private IActionResult Export(List<AccountViewModel> accounts)
    {
        string fileName = "report.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        using (var workbook = new XLWorkbook())
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Accounts report");
            worksheet.Cell(1, 1).Value = "Account Type";
            worksheet.Cell(1, 2).Value = "Account Name";
            worksheet.Cell(1, 3).Value = "Account Number";
            worksheet.Cell(1, 4).Value = "Currency";
            worksheet.Cell(1, 5).Value = "Balance";
            worksheet.Cell(1, 6).Value = "Available";
            worksheet.Cell(1, 7).Value = "Blocked Amount";
            worksheet.Cell(1, 8).Value = "IBAN";

            for (int index = 1; index <= 8; index++)
            {
                worksheet.Cell(1, index).Style.Font.Bold = true;
            }

            for (int index = 1; index <= accounts.Count(); index++)
            {
                worksheet.Cell(index + 1, 1).Value = accounts[index - 1].AccountTypeName;
                worksheet.Cell(index + 1, 2).Value = accounts[index - 1].Name;
                worksheet.Cell(index + 1, 3).Value = accounts[index - 1].Id;
                worksheet.Cell(index + 1, 4).Value = accounts[index - 1].CurrencyAbbreviation;

                worksheet.Cell(index + 1, 5).Value = accounts[index - 1].Balance;
                worksheet.Cell(index + 1, 5).Style.NumberFormat.NumberFormatId = 2;

                worksheet.Cell(index + 1, 6).Value = accounts[index - 1].Available;
                worksheet.Cell(index + 1, 6).Style.NumberFormat.NumberFormatId = 2;

                worksheet.Cell(index + 1, 7).Value = accounts[index - 1].BlockedАmount;
                worksheet.Cell(index + 1, 7).Style.NumberFormat.NumberFormatId = 2;

                worksheet.Cell(index + 1, 8).Value = accounts[index - 1].IBAN;
            }

            IXLRange range = worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(accounts.Count() + 1, 8));
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Dashed;
            range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(0xD4C1D9));

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












        // GET: AccountController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(All));
            }
            catch
            {
                return View();
            }
        }
    }
}
