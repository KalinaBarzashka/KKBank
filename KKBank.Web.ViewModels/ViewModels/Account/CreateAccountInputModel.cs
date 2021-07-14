using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Web.ViewModels.ViewModels.Account
{
    public class CreateAccountInputModel
    {
        public CreateAccountInputModel()
        {
            this.CurrencyItems = new List<KeyValuePair<string, string>>();
            this.AccountTypesItems = new List<KeyValuePair<string, string>>();
        }

        [Required]
        [MaxLength(40, ErrorMessage = "Account name length must be less or equal to 30 characters long.")]
        [MinLength(4, ErrorMessage = "Account name must be at least 4 characters long.")]
        [Display(Name = "Account name")]
        public string Name { get; set; }

        [Display(Name = "Account type")]
        public int AccountTypeId { get; set; }

        [Required]
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CurrencyItems { get; set; }

        public IEnumerable<KeyValuePair<string, string>> AccountTypesItems { get; set; }
    }
}
