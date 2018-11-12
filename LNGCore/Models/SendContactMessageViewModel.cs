using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LNGCore.UI.Models
{
    public class SendContactMessageViewModel
    {
        [Required(ErrorMessage = "Please include your email address, we will need it to contact you back!")]
        [StringLength(100, ErrorMessage = "Wow, your email address is really long! Sorry but it needs to be fewer than 100 characters.")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please include a message, otherwise we won't know the intent of your inquiry!")]
        [StringLength(2000, ErrorMessage = "What a novel! Please keep your message to fewer than 2000 characters.")]
        public string EmailMessage { get; set; }
        [Required]
        public string GoogleClientToken { get; set; }
        public string GooglePublicKey { get; set; }
    }
}
