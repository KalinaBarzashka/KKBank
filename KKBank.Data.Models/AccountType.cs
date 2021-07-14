using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Data.Models
{
    public class AccountType : BaseEntity
    {
        public AccountType()
        {
            this.Accounts = new HashSet<Account>();
            this.AccountRequests = new HashSet<AccountRequest>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [Display(Name = "Account Type Name")]
        public string AccountTypeName { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public virtual ICollection<AccountRequest> AccountRequests { get; set; }
    }
}