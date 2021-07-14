using KKBank.Web.ViewModels.ViewModels.Account;
using KKBank.Web.ViewModels.ViewModels.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KKBank.Services.Data
{
    public interface IRequestService
    {
        Task CreateAddAccountRequestAsync(CreateAccountInputModel input, string userId);

        Task CreateDeleteAccountRequestAsync(DeleteAccountInputModel input, string userId);

        Task ApproveRequestAsync(int requestId, string userId);

        Task DenyRequestAsync(int requestId, string userId);

        Task DenyRequestByUserAsync(int requestId);

        public int GetRequestTypeAddCheckingAccountId();

        public RequestsViewModel<RequestViewModel> GetAllRequestsForUser(string userId);

        public IEnumerable<RequestViewModel> GetFilteredRequestsForUser(string userId, string filterType, string lastNRequests, string toDate, int requestTypeId, int statusId);

        public IEnumerable<BoRequestViewModel> GetFilteredRequestsBO(string filterType, string lastNRequests, string toDate, int requestTypeId, int statusId, string username);

        public IEnumerable<KeyValuePair<string, string>> GetAllActiveRequestTypesAsKeyValuePairs();

        public IEnumerable<AwaitingApprovalReqViewModel> GetAllNewRequests();

        public AwaitingApprovalReqViewModel GetSpecificNewRequest(int id);

        public RequestsViewModel<BoRequestViewModel> GetAllRequestsBO();

        public int RequestsCountForUser(string userId);

        public IEnumerable<RequestViewModel> GetRequestsByIds(List<int> requests);

        public IEnumerable<BoRequestViewModel> GetBoRequestsByIds(List<int> requests);

        public EditRequestViewModel GetRequestById(int id);

        public bool ValidateEditRequest(string requestTypeName, string description, string accName, int currencyId);

        public Task<bool> HandleEditRequest(int requestId, string requestTypeName, string description, string accName, int currencyId);
    }
}