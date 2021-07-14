using KKBank.Services.Data;
using KKBank.Web.ViewModels.ViewModels.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KKBank.Web.BO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public IActionResult Index()
        {
            var viewModel = new StatisticsViewModel();
            this.statisticsService.GetStatistics(viewModel);

            return View(viewModel);
        }
    }
}
