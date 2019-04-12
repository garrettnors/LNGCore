using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Database
{
    public partial class Customer
    {
        public Customer()
        {
            Invoice = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SecondaryEmail { get; set; }
        public string BusinessPhone { get; set; }
        public string AltPhone { get; set; }
        public string Street { get; set; }
        public string PostBox { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool Taxable { get; set; }
        public string TaxId { get; set; }
        public string Balance { get; set; }
        public DateTime? LastContact { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
