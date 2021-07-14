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

namespace KKBank.Web.BO.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class RequestController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRequestService requestService;
        private readonly IStatusService statusService;

        public RequestController(UserManager<ApplicationUser> userManager, 
            IRequestService requestService,
            IStatusService statusService)
        {
            this.userManager = userManager;
            this.requestService = requestService;
            this.statusService = statusService;
        }

        public IActionResult AwaitingApproval()
        {
            var viewModel = requestService.GetAllNewRequests();
            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var viewModel = requestService.GetSpecificNewRequest(id);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            if(id == 0)
            {
                var viewModel = requestService.GetAllNewRequests();
                return this.View(viewModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.requestService.ApproveRequestAsync(id, user.Id);
            return this.Redirect("/Request/Archive");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deny(int id)
        {
            if (id == 0)
            {
                var viewModel = requestService.GetAllNewRequests();
                return this.View(viewModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.requestService.DenyRequestAsync(id, user.Id);
            return this.Redirect("/Request/Archive");
        }

        public IActionResult Archive()
        {
            var viewModel = requestService.GetAllRequestsBO();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Archive(RequestsViewModel<BoRequestViewModel> input)
        {
            if (!this.ModelState.IsValid)
            {
                input.RequestTypesItems = this.requestService.GetAllActiveRequestTypesAsKeyValuePairs();
                input.StatusItems = this.statusService.GetAllActiveStatusAsKeyValuePairs();
                return this.View(input);
            }

            string filterType = input.FilerType;
            string lastNRequests = input.LastNRequests;
            string toDate = input.ToDate;
            int requestTypeId = input.RequestTypeId;
            int statusId = input.StatusId;
            string username = input.UserName;

            var requests = this.requestService.GetFilteredRequestsBO(filterType, lastNRequests, toDate, requestTypeId, statusId, username);

            input.Requests = requests;
            input.RequestTypesItems = this.requestService.GetAllActiveRequestTypesAsKeyValuePairs();
            input.StatusItems = this.statusService.GetAllActiveStatusAsKeyValuePairs();
            return this.View(input);
        }

        [HttpPost]
        public IActionResult Export(List<int> requests)
        {
            var requestsData = this.requestService.GetBoRequestsByIds(requests);
            return this.Export(requestsData);
        }
       
        private IActionResult Export(IEnumerable<BoRequestViewModel> requests)
        {
            string fileName = "transactions.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
       
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Requests report");
                worksheet.Cell(1, 1).Value = "Client Name";
                worksheet.Cell(1, 2).Value = "Account Name";
                worksheet.Cell(1, 3).Value = "Account Type Name";
                worksheet.Cell(1, 4).Value = "Currency Name";
                worksheet.Cell(1, 5).Value = "Description";
                worksheet.Cell(1, 6).Value = "Status Name";
                worksheet.Cell(1, 7).Value = "Request Type Name";
                worksheet.Cell(1, 8).Value = "Created On";
                worksheet.Cell(1, 9).Value = "Signed From Bank Employee";
                worksheet.Cell(1, 10).Value = "Last Modified On";
       
                for (int index = 1; index <= 10; index++)
                {
                    worksheet.Cell(1, index).Style.Font.Bold = true;
                }
       
                var listRequests = requests.ToList();
       
                for (int index = 2; index <= requests.Count(); index++)
                {
                    worksheet.Cell(index, 1).Value = listRequests[index - 1].UserName;
                    worksheet.Cell(index, 2).Value = listRequests[index - 1].AccountName;
                    worksheet.Cell(index, 3).Value = listRequests[index - 1].AccountTypeName;
                    worksheet.Cell(index, 4).Value = listRequests[index - 1].CurrencyName;
                    worksheet.Cell(index, 5).Value = listRequests[index - 1].Description;
                    worksheet.Cell(index, 6).Value = listRequests[index - 1].StatusName;
                    worksheet.Cell(index, 7).Value = listRequests[index - 1].RequestTypeName;
                    worksheet.Cell(index, 8).Value = listRequests[index - 1].CreatedOn;
                    worksheet.Cell(index, 9).Value = listRequests[index - 1].SignedFromBankEmployeeName;
                    worksheet.Cell(index, 10).Value = listRequests[index - 1].ModifiedOn;
                }
       
                IXLRange range = worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(listRequests.Count(), 10));
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
