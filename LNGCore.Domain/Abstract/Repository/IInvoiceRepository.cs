using LNGCore.Domain.Abstract.Class;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Abstract.Repository
{
    public interface IInvoiceRepository
    {
        IEnumerable<IInvoice> GetInvoices();
        IEnumerable<IInvoice> GetInvoicesByCustomer(int customerId);
        IInvoice GetInvoice(int invoiceId);
    }
}
