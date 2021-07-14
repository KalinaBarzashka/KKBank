using System.ComponentModel;

namespace KKBank.Web.ViewModels.ViewModels.Request
{
    public class AwaitingApprovalReqViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        [DisplayName("Request Type Name")]
        public string RequestTypeName { get; set; }

        [DisplayName("Account Type Name")]
        public string AccountTypeName { get; set; }

        public string Currency { get; set; }

        [DisplayName("Status Name")]
        public string StatusName { get; set; }

        public string Description { get; set; }
    }
}
