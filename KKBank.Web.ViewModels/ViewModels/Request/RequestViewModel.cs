using System;

namespace KKBank.Web.ViewModels.ViewModels.Request
{
    public class RequestViewModel
    {
        public int Id { get; set; }

        public string RequestTypeName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string StatusName { get; set; }
    }
}
