using KKBank.Data;
using KKBank.Data.Models;
using KKBank.Web.ViewModels.ViewModels.Account;
using KKBank.Web.ViewModels.ViewModels.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KKBank.Services.Data
{
    public class RequestService : IRequestService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IStatusService statusService;
        private readonly ICurrencyService currencyService;

        public RequestService(ApplicationDbContext dbContext, IStatusService statusService, ICurrencyService currencyService)
        {
            this.dbContext = dbContext;
            this.statusService = statusService;
            this.currencyService = currencyService;
        }

        public async Task CreateAddAccountRequestAsync(CreateAccountInputModel input, string userId)
        {
            int accTypeId = this.dbContext.AccountTypes.Where(x => x.AccountTypeName == "Checking Account").Select(x => x.Id).FirstOrDefault();
            int statusId = statusService.GetAwaitingApprovalStatusId();
            int requestTypeId = GetRequestTypeAddCheckingAccountId();

            var addAccountRequest = new AccountRequest
            {
                UserId = userId,
                AccountName = input.Name,
                AccountTypeId = accTypeId,
                CurrencyId = input.CurrencyId,
                StatusId = statusId,
                RequestTypeId = requestTypeId,
                CreatedOn_17118069 = DateTime.UtcNow
            };

            await this.dbContext.AccountRequests.AddAsync(addAccountRequest);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task CreateDeleteAccountRequestAsync(DeleteAccountInputModel input, string userId)
        {
            int accTypeId = this.dbContext.AccountTypes.Where(x => x.AccountTypeName == "Checking Account").Select(x => x.Id).FirstOrDefault();
            int statusId = this.dbContext.AccountRequestStatus.Where(x => x.Name == "Awaiting Approval").Select(x => x.Id).FirstOrDefault();
            int requestTypeId = GetRequestTypeDeleteCheckingAccountId();

            var accountToDelete = this.dbContext.Accounts.AsNoTracking().Where(x => x.Id == input.AccountId).FirstOrDefault();

            var deleteAccountRequest = new AccountRequest
            {
                UserId = userId,
                AccountName = accountToDelete.Name,
                AccountId = accountToDelete.Id,
                AccountTypeId = accTypeId,
                CurrencyId = accountToDelete.CurrencyId,
                Description = input.Description,
                StatusId = statusId,
                RequestTypeId = requestTypeId,
                CreatedOn_17118069 = DateTime.UtcNow
            };

            await this.dbContext.AccountRequests.AddAsync(deleteAccountRequest);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task ApproveRequestAsync(int id, string userId)
        {
            var accRequest = this.dbContext.AccountRequests.Include(x => x.RequestType).Where(x => x.Id == id).FirstOrDefault();
            
            var approveStatusId = this.statusService.GetApprovedStatusId();

            if (accRequest.RequestType.Name == "Add Checking Account")
            {
                await CreateAccFromRequest(accRequest, userId, approveStatusId);
            }
            else
            {
                await DeleteAccFromRequest(accRequest, userId, approveStatusId);
            }
        }

        public async Task CreateAccFromRequest(AccountRequest request, string userId, int statusId)
        {
            request.SignedFromBankEmployeeId = userId;
            request.StatusId = statusId;

            var newAccount = new Account
            {
                Name = request.AccountName,
                AccountTypeId = request.AccountTypeId,
                CurrencyId = request.CurrencyId,
                UserId = request.UserId,
                CreatedOn_17118069 = DateTime.UtcNow
            };

            await this.dbContext.Accounts.AddAsync(newAccount);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAccFromRequest(AccountRequest request, string userId, int statusId)
        {
            request.SignedFromBankEmployeeId = userId;
            request.StatusId = statusId;

            var accountId = request.AccountId;
            if(accountId != null)
            {
                var account = dbContext.Accounts.Where(x => x.Id == accountId).FirstOrDefault();
                account.IsDeleted_17118069 = true;
                account.DeletedOn_17118069 = DateTime.UtcNow;

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DenyRequestAsync(int requestId, string userId)
        {
            var request = this.dbContext.AccountRequests.Include(x => x.RequestType).Where(x => x.Id == requestId).FirstOrDefault();

            var denyStatusId = this.statusService.GetDenyByBankStatusId();

            request.SignedFromBankEmployeeId = userId;
            request.StatusId = denyStatusId;

            await dbContext.SaveChangesAsync();
        }

        public async Task DenyRequestByUserAsync(int requestId)
        {
            var request = this.dbContext.AccountRequests.Include(x => x.RequestType).Where(x => x.Id == requestId).FirstOrDefault();

            var denyStatusId = this.statusService.GetDenyByUserStatusId();

            request.StatusId = denyStatusId;

            await dbContext.SaveChangesAsync();
        }

        public int GetRequestTypeAddCheckingAccountId()
        {
            return this.dbContext.RequestTypes.Where(x => x.Name == "Add Checking Account").Select(x => x.Id).FirstOrDefault();
        }

        public int GetRequestTypeDeleteCheckingAccountId()
        {
            return this.dbContext.RequestTypes.Where(x => x.Name == "Delete Checking Account").Select(x => x.Id).FirstOrDefault();
        }

        public RequestsViewModel<RequestViewModel> GetAllRequestsForUser(string userId)
        {
            var requests = dbContext.AccountRequests.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .Take(10)
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    RequestTypeName = x.RequestType.Name,
                    CreatedOn = x.CreatedOn_17118069,
                    StatusName = x.Status.Name
                }).ToList();

            var viewModel = new RequestsViewModel<RequestViewModel>
            {
                Requests = requests,
                RequestTypesItems = GetAllActiveRequestTypesAsKeyValuePairs(),
                StatusItems = this.statusService.GetAllActiveStatusAsKeyValuePairs()
            };

            return viewModel;
        }

        public IEnumerable<RequestViewModel> GetFilteredRequestsForUser(string userId, string filterType, string lastNRequests, string toDate, int requestTypeId, int statusId)
        {
            var lastNQuery = String.Empty;
            var dateQuery = String.Empty;
            var reqTypeQuery = String.Empty;
            var statusQuery = String.Empty;
            var offset = String.Empty;

            if (filterType == "1")
            {
                lastNQuery = $" TOP({lastNRequests})";
            }
            else if (filterType == "2")
            {
                var date = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                dateQuery = $" and CAST(ar.CreatedOn_17118069 as date) = '{date}'";
                offset = " OFFSET 0 ROWS";
            }

            if (requestTypeId > 0)
            {
                reqTypeQuery = $" and ar.RequestTypeId = '{requestTypeId}'";
            }

            if (statusId > 0)
            {
                statusQuery = $" and ar.StatusId = '{statusId}'";
            }

            string query = @$"
                SELECT{lastNQuery} *
                FROM [KKBank].[17118069].[AccountRequests] ar
                WHERE ar.UserId = '{userId}'{dateQuery}{reqTypeQuery}{statusQuery}
                ORDER BY ar.CreatedOn_17118069 DESC{offset}
                ";

            var viewModel = dbContext.AccountRequests.FromSqlRaw(query)
                .AsNoTracking()
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    RequestTypeName = x.RequestType.Name,
                    CreatedOn = x.CreatedOn_17118069,
                    StatusName = x.Status.Name
                }).ToList();

            return viewModel;
        }

        public IEnumerable<BoRequestViewModel> GetFilteredRequestsBO(string filterType, string lastNRequests, string toDate, int requestTypeId, int statusId, string username)
        {
            var lastNQuery = String.Empty;
            var dateQuery = String.Empty;
            var reqTypeQuery = String.Empty;
            var statusQuery = String.Empty;
            var offset = String.Empty;

            if (filterType == "1")
            {
                lastNQuery = $" TOP({lastNRequests})";
            }
            else if (filterType == "2")
            {
                var date = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                dateQuery = $" and CAST(ar.CreatedOn_17118069 as date) = '{date}'";
                offset = " OFFSET 0 ROWS";
            }

            if (requestTypeId > 0)
            {
                reqTypeQuery = $" and ar.RequestTypeId = '{requestTypeId}'";
            }

            if (statusId > 0)
            {
                statusQuery = $" and ar.StatusId = '{statusId}'";
            }

            string query = @$"";

            if (!String.IsNullOrEmpty(username))
            {
                query += @$"SELECT{lastNQuery} ar.*, u.UserName
                            FROM [KKBank].[17118069].[AccountRequests] ar 
                            INNER JOIN [AspNetUsers] u ON ar.UserId = u.Id 
                            WHERE u.UserName like '%{username}%'{dateQuery}{reqTypeQuery}{statusQuery}";
            }
            else
            {
                query += $@"SELECT{lastNQuery} ar.*
                            FROM [KKBank].[17118069].[AccountRequests] ar ";
                string where = @$"{dateQuery}{reqTypeQuery}{statusQuery}";
                if (where.StartsWith(" and"))
                {
                    where = where.Substring(4);
                }
                if (where.Length > 0)
                {
                    query += ("WHERE" + where);
                }
            }
            
            query += @$" ORDER BY ar.CreatedOn_17118069 DESC{offset}";

            var viewModel = dbContext.AccountRequests.FromSqlRaw(query)
                .AsNoTracking()
                .Select(x => new BoRequestViewModel
                {
                    Id = x.Id,
                    UserName = x.User.UserName,
                    AccountName = x.Account.Name,
                    AccountTypeName = x.AccountType.AccountTypeName,
                    CurrencyName = x.Currency.CurrencyAbbreviation,
                    Description = x.Description,
                    StatusName = x.Status.Name,
                    RequestTypeName = x.RequestType.Name,
                    CreatedOn = x.CreatedOn_17118069,
                    SignedFromBankEmployeeName = x.SignedFromBankEmployee.UserName,
                    ModifiedOn = x.ModifiedOn_17118069 != null ? x.ModifiedOn_17118069.Value.ToString("dd/MM/yyyy HH:mm:ss") : null
                }).ToList();

            return viewModel;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllActiveRequestTypesAsKeyValuePairs()
        {
            return this.dbContext.RequestTypes.Where(x => x.IsDeleted_17118069 == false).Select(x => new
            {
                x.Id,
                x.Name
            }).ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public IEnumerable<AwaitingApprovalReqViewModel> GetAllNewRequests()
        {
            var statusId = statusService.GetAwaitingApprovalStatusId();
            var viewModel = dbContext.AccountRequests.Where(x => x.StatusId == statusId)
                .OrderByDescending(x => x.Id)
                .Select(x => new AwaitingApprovalReqViewModel
                {
                    Id = x.Id,
                    Username = x.User.UserName,
                    RequestTypeName = x.RequestType.Name,
                    Currency = x.Currency.CurrencyAbbreviation
                }).ToList();

            return viewModel;
        }

        public AwaitingApprovalReqViewModel GetSpecificNewRequest(int id)
        {
            var statusId = statusService.GetAwaitingApprovalStatusId();
            var viewModel = dbContext.AccountRequests
                .Where(x => x.StatusId == statusId && x.Id == id)
                .Select(x => new AwaitingApprovalReqViewModel
                {
                    Id = x.Id,
                    Username = x.User.UserName,
                    AccountName = x.AccountName,
                    RequestTypeName = x.RequestType.Name,
                    AccountTypeName = x.AccountType.AccountTypeName,
                    Currency = x.Currency.CurrencyAbbreviation,
                    Description = x.Description,
                    StatusName = x.Status.Name
                }).FirstOrDefault();

            return viewModel;
        }

        public RequestsViewModel<BoRequestViewModel> GetAllRequestsBO()
        {
            var awaitingApprovalStatus = this.statusService.GetAwaitingApprovalStatusId();
            var requests = dbContext.AccountRequests
                .OrderByDescending(x => x.Id)
                .Select(x => new BoRequestViewModel
                {
                    Id = x.Id,
                    UserName = x.User.UserName,
                    AccountName = x.Account.Name,
                    AccountTypeName = x.AccountType.AccountTypeName,
                    CurrencyName = x.Currency.CurrencyAbbreviation,
                    Description = x.Description,
                    StatusName = x.Status.Name,
                    RequestTypeName = x.RequestType.Name,
                    CreatedOn = x.CreatedOn_17118069,
                    SignedFromBankEmployeeName = x.SignedFromBankEmployee.UserName,
                    ModifiedOn = x.ModifiedOn_17118069 != null ? x.ModifiedOn_17118069.Value.ToString("dd/MM/yyyy HH:mm:ss") : null
                }).ToList();

            var viewModel = new RequestsViewModel<BoRequestViewModel>
            {
                Requests = requests,
                RequestTypesItems = GetAllActiveRequestTypesAsKeyValuePairs(),
                StatusItems = this.statusService.GetAllActiveStatusAsKeyValuePairs()
            };

            return viewModel;
        }

        public int RequestsCountForUser(string userId)
        {
            return this.dbContext.AccountRequests.Where(x => x.UserId == userId).Count();

        }

        public IEnumerable<RequestViewModel> GetRequestsByIds(List<int> requests)
        {
            return this.dbContext.AccountRequests
                .Where(x => requests.Contains(x.Id))
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    RequestTypeName = x.RequestType.Name,
                    CreatedOn = x.CreatedOn_17118069,
                    StatusName = x.Status.Name
                })
                .ToList();
        }

        public IEnumerable<BoRequestViewModel> GetBoRequestsByIds(List<int> requests)
        {
            return this.dbContext.AccountRequests
                .Where(x => requests.Contains(x.Id))
                .Select(x => new BoRequestViewModel
                {
                    Id = x.Id,
                    UserName = x.User.UserName,
                    AccountName = x.Account.Name,
                    AccountTypeName = x.AccountType.AccountTypeName,
                    CurrencyName = x.Currency.CurrencyAbbreviation,
                    Description = x.Description,
                    StatusName = x.Status.Name,
                    RequestTypeName = x.RequestType.Name,
                    CreatedOn = x.CreatedOn_17118069,
                    SignedFromBankEmployeeName = x.SignedFromBankEmployee.UserName,
                    ModifiedOn = x.ModifiedOn_17118069 != null ? x.ModifiedOn_17118069.Value.ToString("dd/MM/yyyy HH:mm:ss") : null
                })
                .ToList();
        }

        public EditRequestViewModel GetRequestById(int id)
        {
            return this.dbContext.AccountRequests
                .Where(x => x.Id == id)
                .Select(x => new EditRequestViewModel
                {
                    Id = x.Id,
                    AccountName = x.AccountName,
                    AccountId = x.AccountId,
                    AccountTypeName = x.AccountType.AccountTypeName,
                    CurrencyId = x.CurrencyId,
                    CurrencyName = x.Currency.CurrencyAbbreviation,
                    Description = x.Description,
                    RequestTypeName = x.RequestType.Name,
                    CreatedOn = x.CreatedOn_17118069,
                    ModifiedOn = x.ModifiedOn_17118069.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    StatusName = x.Status.Name,
                    IsDelete = x.AccountId != null ? true : false
                }).FirstOrDefault();
        }

        private async Task<bool> EditDeleteAccountRequest(int requestId, string reasonForDeletion)
        {
            var request = dbContext.AccountRequests.Where(x => x.Id == requestId).FirstOrDefault();
            request.Description = reasonForDeletion;

            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> EditAddAccountRequest(int requestId, string accName, int currencyId)
        {
            var request = dbContext.AccountRequests.Where(x => x.Id == requestId).FirstOrDefault();
            request.AccountName = accName;
            request.CurrencyId = currencyId;

            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ValidateEditRequest(string requestTypeName, string description, string accName, int currencyId)
        {
            if (requestTypeName == "Delete Checking Account")
            {
                if (description.Length < 40)
                {
                    return false;
                }
            }
            else if (requestTypeName == "Add Checking Account")
            {
                var currencyIds = this.currencyService.GetAllActiveCurrencyIds();
                if (accName.Length < 4 || accName.Length > 40 || !currencyIds.Contains(currencyId))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> HandleEditRequest(int requestId, string requestTypeName, string description, string accName, int currencyId)
        {
            if (requestTypeName == "Delete Checking Account")
            {
                var success = await EditDeleteAccountRequest(requestId, description);
                if (success)
                {
                    return true;
                }
            }
            else if (requestTypeName == "Add Checking Account")
            {
                var success = await EditAddAccountRequest(requestId, accName, currencyId);
                if (success)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
