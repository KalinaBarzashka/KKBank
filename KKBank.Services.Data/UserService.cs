using KKBank.Data;
using KKBank.Web.ViewModels.ViewModels.User;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KKBank.Services.Data
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAccountService accountService;

        public UserService(ApplicationDbContext dbContext,
            IAccountService accountService)
        {
            this.dbContext = dbContext;
            this.accountService = accountService;
        }

        public string GetFirstAndLastNameOfUser(string userId)
        {
            var userFirstLastName = this.dbContext.ApplicationUsers.Where(x => x.Id == userId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();

            return userFirstLastName;
        }

        public string GetUserIdByEGN(string EGN)
        {
            return this.dbContext.ApplicationUsers
                .Where(x => x.EGN == EGN)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public void GetUserInfoAndAccountsByUserId(string id, UserAddMoneyViewModel input)
        {
            input.HasData = true;
            var user = this.dbContext.ApplicationUsers
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            input.FirstName = user.FirstName;
            input.MiddleName = user.MiddleName;
            input.LastName = user.LastName;

            input.UserAccounts = this.accountService.GetActiveAccountsForUser(id);
        }
    }
}
