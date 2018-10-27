using LNGCore.Domain.Abstract.Class;
using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Concrete
{
    public partial class Customer : ICustomer
    {
        public Customer()
        {
            Invoice = new List<Invoice>();
        }

        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
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
        public IEnumerable<Invoice> Invoice { get; set; }
        IEnumerable<IInvoice> ICustomer.Invoice { get { return Invoice; } set => throw new NotImplementedException(); }
    }
}
