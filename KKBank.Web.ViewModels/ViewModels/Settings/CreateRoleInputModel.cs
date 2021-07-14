using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KKBank.Web.ViewModels.ViewModels.Settings
{
    public class CreateRoleInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}
