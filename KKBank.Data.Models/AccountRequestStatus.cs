using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Data.Models
{
    public class AccountRequestStatus : BaseEntity
    {
        public AccountRequestStatus()
        {
            this.AccountRequests = new HashSet<AccountRequest>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public virtual ICollection<AccountRequest> AccountRequests { get; set; }

    }
}
