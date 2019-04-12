using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Database;

namespace LNGCore.UI.Models.Admin
{
    public class EditCustomerViewModel
    {
        public Customer Customer { get; set; }
        public bool? ShowSuccessMessage { get; set; }        
    }
}
