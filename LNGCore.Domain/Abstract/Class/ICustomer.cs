using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Abstract.Class
{
    public interface ICustomer
    {
        string AltPhone { get; set; }
        string Balance { get; set; }
        string BusinessName { get; set; }
        string BusinessPhone { get; set; }
        string City { get; set; }
        string Email { get; set; }
        int Id { get; set; }
        DateTime? LastContact { get; set; }
        string Name { get; set; }
        string PostBox { get; set; }
        string State { get; set; }
        string Street { get; set; }
        bool Taxable { get; set; }
        string TaxId { get; set; }
        string ZipCode { get; set; }
        IEnumerable<IInvoice> Invoice { get; set; }
    }
}