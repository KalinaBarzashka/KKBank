using KKBank.Web.ViewModels.ViewModels.Payments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KKBank.Services.Data
{
    public interface IPaymentsService
    {
        public PaymentsBetweenOwnAccountsViewModel FillBetweenOwnAccountsViewModel(string userId);

        public bool ValidateInputBetweenOwnAccountsPayment(int accountFromId, int accountToId, string amount, string userId, string description);

        public Task ExecuteBetweenOwnAccountsPayment(int accountFromId, int accountToId, decimal fromAmount, string userId, string description);

        public PaymentsToKKBankAccountViewModel FillToKKBankAccountViewModel(string userId);

        public bool ValidateInputToKKBankAccountPayment(string userId, int accountFromId, string IBAN, string beneficiaryName, int transferCurrencyId, string amountFrom, string amountTo, string paymentDetails);

        public Task ExecuteToKKBankAccountPayment(string userId, int accountFromId, string IBAN, string beneficiaryName, int transferCurrencyId, decimal amountFrom, decimal amountTo, string paymentDetails, string additionalDetails);

        public PaymentsViewModel GetAllPaymentsForUser(string userId);

        public IEnumerable<PaymentViewModel> GetFilteredPaymentsForUser(string userId, string filterType, string lastNRequests, string toDate, int statusId);

        public IEnumerable<PaymentViewModel> GetPaymentsByIds(List<int> payments);
    }
}
