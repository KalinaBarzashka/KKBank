using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KKBank.Data.Models
{
    public class AccountRequest : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string AccountName { get; set; }

        public int? AccountId { get; set; }
        public Account Account { get; set; }

        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public string Description { get; set; }

        public int StatusId { get; set; }
        public AccountRequestStatus Status { get; set; }

        public int RequestTypeId { get; set; }
        public RequestType RequestType { get; set; }


        public string SignedFromBankEmployeeId { get; set; }
        public virtual ApplicationUser SignedFromBankEmployee { get; set; }
    }
}
