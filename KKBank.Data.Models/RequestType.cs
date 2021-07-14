using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Data.Models
{
    public class RequestType : BaseEntity
    {
        public RequestType()
        {
            this.AccountRequests = new HashSet<AccountRequest>();
        }
        
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
    
        public virtual ICollection<AccountRequest> AccountRequests { get; set; }
    }
    
}