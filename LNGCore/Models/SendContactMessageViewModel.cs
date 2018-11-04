using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models
{
    public class SendContactMessageViewModel
    {
        [Required]
        [MaxLength(100)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(2000)]
        public string EmailMessage { get; set; }
    }
}
