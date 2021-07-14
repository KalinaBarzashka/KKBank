using System;
using System.ComponentModel.DataAnnotations;

namespace KKBank.Data.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TableName { get; set; }

        [Required]
        public string Operation { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
