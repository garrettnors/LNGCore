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
        IEnumerable<ILineItem> GetLineItems(int invoiceId, int startingIndex, int roundToNearest);
        IEnumerable<IItem> GetItemTypes();
        void SaveLineItems(List<ILineItem> lines, int invoiceId);
        IInvoice GetInvoice(int invoiceId);
        int SaveInvoice(IInvoice invoice);
        bool MarkInvoicePaid(int invoiceId);
    }
}
