using KKBank.Web.ViewModels.ViewModels.Statistics;

namespace KKBank.Services.Data
{
    public interface IStatisticsService
    {
        public void GetStatistics(StatisticsViewModel viewModel);
    }
}
