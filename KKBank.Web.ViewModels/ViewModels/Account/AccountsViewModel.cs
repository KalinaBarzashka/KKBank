using System.Collections.Generic;

namespace KKBank.Web.ViewModels.ViewModels.Account
{
    public class AccountsViewModel : PagingViewModel
    {
        public AccountsViewModel()
        {
            this.Accounts = new List<AccountViewModel>();
        }

        public IEnumerable<AccountViewModel> Accounts { get; set; }
    }
}
