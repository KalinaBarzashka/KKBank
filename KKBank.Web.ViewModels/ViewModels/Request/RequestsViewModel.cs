using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Web.ViewModels.ViewModels.Request
{
    public class RequestsViewModel<T>
    {
        public RequestsViewModel()
        {
            this.Requests = new List<T>();
            this.RequestTypesItems = new List<KeyValuePair<string, string>>();
            this.StatusItems = new List<KeyValuePair<string, string>>();
        }
        public IEnumerable<T> Requests { get; set; }

        public int RequestTypeId { get; set; }
        public IEnumerable<KeyValuePair<string, string>> RequestTypesItems { get; set; }

        public int StatusId { get; set; }
        public IEnumerable<KeyValuePair<string, string>> StatusItems { get; set; }

        [MaxLength(256)]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string FilerType { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "The field must contain only digits!")]
        [RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Please enter a number!")]
        [MaxLength(3)]
        public string LastNRequests { get; set; }

        public string ToDate { get; set; }
    }
}
