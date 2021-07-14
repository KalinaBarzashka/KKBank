using System.Collections.Generic;

namespace KKBank.Services.Data
{
    public interface IStatusService
    {
        public IEnumerable<KeyValuePair<string, string>> GetAllActiveStatusAsKeyValuePairs();

        public IEnumerable<KeyValuePair<string, string>> GetAllActivePaymentOrderStatusAsKeyValuePairs();

        public int GetAwaitingApprovalStatusId();

        public int GetApprovedStatusId();

        public int GetDenyByBankStatusId();

        public int GetDenyByUserStatusId();

        public int GetOperationExecutedStatusId();
    }
}
