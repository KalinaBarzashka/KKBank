using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Web.ViewModels.ViewModels.Account
{
    public class DeleteAccountInputModel
    {
        public DeleteAccountInputModel()
        {
            this.UserAccounts = new List<KeyValuePair<string, string>>();
        }

        [Required]
        [Display(Name = "Reason for Deletion")]
        [MinLength(40, ErrorMessage = "The reason for deleting the selected account must be at least 30 characters long.")]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select account to delete.")]
        [Display(Name = "Account")]
        public int AccountId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> UserAccounts { get; set; }


    }
}
