using KKBank.Web.ViewModels.ViewModels.Account;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Web.ViewModels.ViewModels.User
{
    public class UserAddMoneyViewModel
    {
        public bool HasData { get; set; }

        [RegularExpression("[0-9]{2}[0,1,2,4][0-9][0-9]{2}[0-9]{4}")]
        [MaxLength(10)]
        public string EGN { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<AccountViewModel> UserAccounts { get; set; }
    }
}
