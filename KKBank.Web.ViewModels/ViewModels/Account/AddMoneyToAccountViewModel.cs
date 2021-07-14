using System.ComponentModel.DataAnnotations;

namespace KKBank.Web.ViewModels.ViewModels.Account
{
    public class AddMoneyToAccountViewModel
    {
        [Required]
        [MaxLength(14)]
        [RegularExpression("^[0-9]{1,11}[.|,]{0,1}[0-9]{0,2}$", ErrorMessage = "Please enter a valid amount greater than '0.00'. Valid amounts are from 1 to 11 digits, decimal point. Examples: '123.45', '6.15', '0.10'.")]
        [Range(0.01, 99999999999.99, ErrorMessage = "Please enter a valid amount greater than '0.00'. Valid amounts are from 1 to 14 digits, decimal point.")]
        public string Amount { get; set; }

        public int AccountId { get; set; }

        public AccountViewModel Account { get; set; }

        public string EGN { get; set; }
    }
}
