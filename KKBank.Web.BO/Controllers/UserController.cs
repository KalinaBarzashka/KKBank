using KKBank.Services.Data;
using KKBank.Web.ViewModels.ViewModels.Account;
using KKBank.Web.ViewModels.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KKBank.Web.BO.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IAccountService accountService;

        public UserController(IUserService userService,
            IAccountService accountService)
        {
            this.userService = userService;
            this.accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddMoney()
        {
            var viewModel = new UserAddMoneyViewModel();
            viewModel.HasData = false;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMoney(UserAddMoneyViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.userService.GetUserIdByEGN(input.EGN);
            if(userId == null)
            {
                return this.View(input);
            }

            this.userService.GetUserInfoAndAccountsByUserId(userId, input);
            return View(input);
        }

        public IActionResult AddMoneyToAccount(int id, string EGN)
        {
            var account = this.accountService.GetAccountById(id);

            var viewModel = new AddMoneyToAccountViewModel();
            viewModel.Account = account;
            viewModel.AccountId = account.Id;
            viewModel.EGN = EGN;

            return this.View(viewModel);
            //return this.Redirect("/User/SuccessfulOperation");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMoneyToAccount(AddMoneyToAccountViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            bool isSuccess = await this.accountService.AddMoneyToAccount(input.AccountId, input.Amount);
            if (isSuccess)
            {
                return this.RedirectToAction("SuccessfulOperation", "User", new { input.EGN });
            }

            return this.View();
        }

        public IActionResult SuccessfulOperation(string EGN)
        {
            return this.View("SuccessfulOperation", EGN);
        }
    }
}
