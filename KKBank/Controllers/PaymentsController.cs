using ClosedXML.Excel;
using KKBank.Data.Models;
using KKBank.Services.Data;
using KKBank.Web.ViewModels.ViewModels.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KKBank.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class PaymentsController : Controller
    {
        private readonly IPaymentsService paymentsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStatusService statusService;

        public PaymentsController(IPaymentsService paymentsService,
            UserManager<ApplicationUser> userManager,
            IStatusService statusService)
        {
            this.paymentsService = paymentsService;
            this.userManager = userManager;
            this.statusService = statusService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BetweenOwnAccounts()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.paymentsService.FillBetweenOwnAccountsViewModel(user.Id);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BetweenOwnAccounts(PaymentsBetweenOwnAccountsViewModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var accountFromId = input.FromAccountId;
            var accountToId = input.ToAccountId;
            var amount = input.AmountFrom;
            var descr = input.PaymentDetails;

            bool isValid = paymentsService.ValidateInputBetweenOwnAccountsPayment(accountFromId, accountToId, amount, user.Id, descr);
            if (!this.ModelState.IsValid || !isValid)
            {
                var accountsData = this.paymentsService.FillBetweenOwnAccountsViewModel(user.Id);
                input.FromAccounts = accountsData.FromAccounts;
                input.ToAccounts = accountsData.ToAccounts;

                if (!isValid)
                {
                    this.ModelState.AddModelError("FromAccountId", "Not enough money!");
                }

                return this.View(input);
            }

            decimal fromAmount = decimal.Parse(amount);
            await paymentsService.ExecuteBetweenOwnAccountsPayment(accountFromId, accountToId, fromAmount, user.Id, descr);

            return this.Redirect("/Payments/Index");
        }

        public async Task<IActionResult> InternalTransfer()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.paymentsService.FillToKKBankAccountViewModel(user.Id);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InternalTransfer(PaymentsToKKBankAccountViewModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var accountFromId = input.FromAccountId;
            string iban = input.IBAN;
            string beneficiaryName = input.BeneficiaryName;
            int transferCurrencyId = input.TransferCurrencyId;
            string amountFrom = input.AmountFrom;
            string amountTo = input.AmountTo;
            string details = input.PaymentDetails;
            string additionalDetails = input.AdditionalDetails;

            var isValid = this.paymentsService.ValidateInputToKKBankAccountPayment(user.Id, accountFromId, iban, beneficiaryName, transferCurrencyId, amountFrom, amountTo, details);

            if (!this.ModelState.IsValid || !isValid)
            {
                var data = this.paymentsService.FillToKKBankAccountViewModel(user.Id);

                input.Currency = data.Currency;
                input.FromAccounts = data.FromAccounts;

                if (!isValid)
                {
                    ModelState.AddModelError(nameof(PaymentsToKKBankAccountViewModel), "Input data is invalid! Please check beneficiary’s data!");
                }

                return this.View(input);
            }

            decimal amountFromParsed = decimal.Parse(amountFrom);
            decimal amountToParsed = decimal.Parse(amountTo);
            await paymentsService.ExecuteToKKBankAccountPayment(user.Id, accountFromId, iban, beneficiaryName, transferCurrencyId, amountFromParsed, amountToParsed, details, additionalDetails);

            return this.Redirect("/Payments/Index");
        }

        public async Task<IActionResult> Archive()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = this.paymentsService.GetAllPaymentsForUser(user.Id);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(PaymentsViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.StatusItems = this.statusService.GetAllActivePaymentOrderStatusAsKeyValuePairs();
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            string filterType = input.FilerType;
            string lastNRequests = input.LastNRequests;
            string toDate = input.ToDate;
            int statusId = input.StatusId;

            var payments = this.paymentsService.GetFilteredPaymentsForUser(user.Id, filterType, lastNRequests, toDate, statusId);
            input.Payments = payments;
            input.StatusItems = this.statusService.GetAllActivePaymentOrderStatusAsKeyValuePairs();

            return this.View(input);
        }

        [HttpPost]
        public IActionResult Export(List<int> payments)
        {
            var paymentsData = this.paymentsService.GetPaymentsByIds(payments);
            return this.Export(paymentsData);
        }

        private IActionResult Export(IEnumerable<PaymentViewModel> payments)
        {
            string fileName = "payments.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Payments report");
                worksheet.Cell(1, 1).Value = "Payment Type";
                worksheet.Cell(1, 2).Value = "Payor Info";
                worksheet.Cell(1, 3).Value = "Payee Info";
                worksheet.Cell(1, 4).Value = "Amount";
                worksheet.Cell(1, 5).Value = "CreatedOn";
                worksheet.Cell(1, 6).Value = "Status";

                for (int index = 1; index <= 6; index++)
                {
                    worksheet.Cell(1, index).Style.Font.Bold = true;
                }

                var listPayments = payments.ToList();

                for (int index = 1; index <= payments.Count(); index++)
                {
                    worksheet.Cell(index + 1, 1).Value = listPayments[index - 1].PaymentType;
                    worksheet.Cell(index + 1, 2).Value = listPayments[index - 1].PayorInfo;
                    worksheet.Cell(index + 1, 3).Value = listPayments[index - 1].PayeeInfo;

                    worksheet.Cell(index + 1, 4).Value = listPayments[index - 1].FromAmount;
                    worksheet.Cell(index + 1, 4).Style.NumberFormat.NumberFormatId = 2;

                    worksheet.Cell(index + 1, 5).Value = listPayments[index - 1].CreatedOn;
                    worksheet.Cell(index + 1, 6).Value = listPayments[index - 1].PaymentOrderStatus;
                }

                IXLRange range = worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(payments.Count() + 1, 6));
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                range.Style.Border.InsideBorder = XLBorderStyleValues.Dashed;
                //range.Style.Fill.SetBackgroundColor(XLColor.FromArgb(0xD4C1D9));

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
