using ClosedXML.Excel;
using KKBank.Data.Models;
using KKBank.Services.Data;
using KKBank.Web.ViewModels.ViewModels.Request;
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
    public class RequestController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRequestService requestService;
        private readonly IStatusService statusService;
        private readonly ICurrencyService currencyService;

        public RequestController(UserManager<ApplicationUser> userManager, 
            IRequestService requestService,
            IStatusService statusService,
            ICurrencyService currencyService)
        {
            this.userManager = userManager;
            this.requestService = requestService;
            this.statusService = statusService;
            this.currencyService = currencyService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Archive()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = this.requestService.GetAllRequestsForUser(user.Id);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(RequestsViewModel<RequestViewModel> input)
        {
            if (!this.ModelState.IsValid)
            {
                input.RequestTypesItems = this.requestService.GetAllActiveRequestTypesAsKeyValuePairs();
                input.StatusItems = this.statusService.GetAllActiveStatusAsKeyValuePairs();
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            string filterType = input.FilerType;
            string lastNRequests = input.LastNRequests;
            string toDate = input.ToDate;
            int requestTypeId = input.RequestTypeId;
            int statusId = input.StatusId; 

            var requests = this.requestService.GetFilteredRequestsForUser(user.Id, filterType, lastNRequests, toDate, requestTypeId, statusId);
            input.Requests = requests;

            input.RequestTypesItems = this.requestService.GetAllActiveRequestTypesAsKeyValuePairs();
            input.StatusItems = this.statusService.GetAllActiveStatusAsKeyValuePairs();
            return this.View(input);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Close(int id)
        {
            if(id != 0)
            {
                await requestService.DenyRequestByUserAsync(id);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            return this.Redirect("/Request/Archive");
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Edit(int id)
        {
            if (id != 0)
            {
                var request = this.requestService.GetRequestById(id);
                request.CurrencyItems = this.currencyService.GetAllActiveCurrencyAsKeyValuePairs();
                return this.View(request);
            }

            return this.Redirect("/Request/Archive");
        }

        [HttpPost]
        public async Task<IActionResult> EditRequest(EditRequestViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var request = this.requestService.GetRequestById(input.Id);
                input.CurrencyItems = this.currencyService.GetAllActiveCurrencyAsKeyValuePairs();
                return this.View("Edit", request);
            }

            var requestId = input.Id;
            var requestTypeName = input.RequestTypeName;
            var description = input.Description;
            var accName = input.AccountName;
            var currencyId = input.CurrencyId;

            var valid = this.requestService.ValidateEditRequest(requestTypeName, description, accName, currencyId);
            if (!valid)
            {
                var request = this.requestService.GetRequestById(input.Id);
                input.CurrencyItems = this.currencyService.GetAllActiveCurrencyAsKeyValuePairs();
                return this.View("Edit", request);
            }

            var success = await this.requestService.HandleEditRequest(requestId, requestTypeName, description, accName, currencyId);

            if (success)
            {
                return this.View();
            }

            return this.View("EditRequestError");
        }

        [HttpPost]
        public IActionResult Export(List<int> requests)
        {
            var requestsData = this.requestService.GetRequestsByIds(requests);
            return this.Export(requestsData);
        }

        private IActionResult Export(IEnumerable<RequestViewModel> requests)
        {
            string fileName = "requests.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Requests report");
                worksheet.Cell(1, 1).Value = "Request Id";
                worksheet.Cell(1, 2).Value = "Request Type";
                worksheet.Cell(1, 3).Value = "CreatedOn";
                worksheet.Cell(1, 4).Value = "Status";

                for (int index = 1; index <= 4; index++)
                {
                    worksheet.Cell(1, index).Style.Font.Bold = true;
                }

                var listRequests = requests.ToList();

                for (int index = 1; index <= requests.Count(); index++)
                {
                    worksheet.Cell(index + 1, 1).Value = listRequests[index - 1].Id;
                    worksheet.Cell(index + 1, 2).Value = listRequests[index - 1].RequestTypeName;
                    worksheet.Cell(index + 1, 3).Value = listRequests[index - 1].CreatedOn;
                    worksheet.Cell(index + 1, 4).Value = listRequests[index - 1].StatusName;
                }

                IXLRange range = worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(requests.Count() + 1, 4));
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
