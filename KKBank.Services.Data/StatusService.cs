using KKBank.Data;
using System.Collections.Generic;
using System.Linq;

namespace KKBank.Services.Data
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationDbContext dbContext;

        public StatusService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllActiveStatusAsKeyValuePairs()
        {
            return this.dbContext.AccountRequestStatus.Where(x => x.IsDeleted_17118069 == false).Select(x => new
            {
                x.Id,
                x.Name
            })
            .ToList()
            .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public int GetAwaitingApprovalStatusId()
        {
            return this.dbContext.AccountRequestStatus.Where(x => x.Name == "Awaiting Approval").Select(x => x.Id).FirstOrDefault();
        }

        public int GetApprovedStatusId()
        {
            return this.dbContext.AccountRequestStatus.Where(x => x.Name == "Approved").Select(x => x.Id).FirstOrDefault();
        }

        public int GetDenyByBankStatusId()
        {
            return this.dbContext.AccountRequestStatus.Where(x => x.Name == "Closed by Bank").Select(x => x.Id).FirstOrDefault();
        }

        public int GetDenyByUserStatusId()
        {
            return this.dbContext.AccountRequestStatus.Where(x => x.Name == "Closed by Client").Select(x => x.Id).FirstOrDefault();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllActivePaymentOrderStatusAsKeyValuePairs()
        {
            return this.dbContext.PaymentOrderStatus.Where(x => x.IsDeleted_17118069 == false).Select(x => new
            {
                x.Id,
                x.StatusName
            })
            .ToList()
            .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.StatusName));
        }

        public int GetOperationExecutedStatusId()
        {
            return this.dbContext.PaymentOrderStatus.Where(x => x.StatusName == "Operation executed").Select(x => x.Id).FirstOrDefault();
        }
    }
}
