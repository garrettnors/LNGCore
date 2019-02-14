using LNGCore.Domain.Abstract.Class;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Abstract.Repository
{
    public interface IInvoiceRepository
    {
        IEnumerable<IInvoice> GetOpenInvoices(string searchTerm = "");
        IEnumerable<IInvoice> GetPastDueInvoices(string searchTerm = "");
        IEnumerable<IInvoice> GetPaidInvoices(string searchTerm = "");
        IEnumerable<IInvoice> GetOpenQuotes(string searchTerm = "");
        IEnumerable<IInvoice> GetDonatedItems(string searchTerm = "");
        IEnumerable<IInvoice> GetVoidedItems(string searchTerm = "");
        IEnumerable<IInvoice> GetYearToDateSales();
        IEnumerable<IInvoice> GetInvoicesByCustomer(int customerId);
        IInvoice GetInvoice(int invoiceId);
        bool MarkInvoicePaid(int invoiceId);
    }
}
