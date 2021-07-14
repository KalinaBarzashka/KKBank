using KKBank.Web.ViewModels.ViewModels.User;

namespace KKBank.Services.Data
{
    public interface IUserService
    {
        public string GetFirstAndLastNameOfUser(string userId);

        public string GetUserIdByEGN(string EGN);

        public void GetUserInfoAndAccountsByUserId(string id, UserAddMoneyViewModel input);
    }
}
