using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.UI.Models.Admin
{
    public class EditCustomerViewModel : ICustomer
    {
        [DisplayName("Alternate Phone")]
        [StringLength(50)]
        public string AltPhone { get; set; }
        [StringLength(50)]
        public string Balance { get; set; }
        [DisplayName("Business Name")]
        [StringLength(50)]
        public string BusinessName { get; set; }
        [DisplayName("Business Phone")]
        [StringLength(50)]
        public string BusinessPhone { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public int Id { get; set; }
        [DisplayName("Last Contact")]
        [StringLength(50)]
        public DateTime? LastContact { get; set; }
        [DisplayName("Contact Name")]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        [DisplayName("Post Box")]
        public string PostBox { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(50)]
        public string Street { get; set; }
        public bool Taxable { get; set; }
        [DisplayName("Tax Id")]
        [StringLength(50)]
        public string TaxId { get; set; }
        [DisplayName("Zip Code")]
        [StringLength(50)]
        public string ZipCode { get; set; }
        public IEnumerable<IInvoice> Invoice { get; set; }
        public string DisplayName { get; }
    }
}
