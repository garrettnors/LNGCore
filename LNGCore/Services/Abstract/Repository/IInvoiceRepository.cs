using LNGCore.Domain.Abstract.Class;
using Microsoft.AspNetCore.Http;
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
        IEnumerable<ILineItem> GetLineItems(string searchTerm, int? customerId = null);
        IEnumerable<IItem> GetItemTypes();
        void SaveLineItems(List<ILineItem> lines, int invoiceId);
        IInvoice GetInvoice(int invoiceId);
        int SaveInvoice(IInvoice invoice);
        bool MarkInvoicePaid(int invoiceId);
        string SaveAttachmentsToInvoice(int invoiceId, List<IFormFile> files, bool customerCanSee);
    }
}
