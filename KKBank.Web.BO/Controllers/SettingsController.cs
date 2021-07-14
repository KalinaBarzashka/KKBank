using KKBank.Services.Data;
using KKBank.Web.ViewModels.ViewModels.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KKBank.Web.BO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly ISettingsService settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateRole()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleInputModel input)
        {
            await this.settingsService.CreateRole(input.Name);
            return this.Redirect("/Settings");
        }
    }
}
