using LNGCore.Domain.Database;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface IInvoiceService
    {
        IEnumerable<Invoice> GetOpenInvoices(string searchTerm = "");
        IEnumerable<Invoice> GetPastDueInvoices(string searchTerm = "");
        IEnumerable<Invoice> GetPaidInvoices(string searchTerm = "");
        IEnumerable<Invoice> GetOpenQuotes(string searchTerm = "");
        IEnumerable<Invoice> GetDonatedItems(string searchTerm = "");
        IEnumerable<Invoice> GetVoidedItems(string searchTerm = "");
        IEnumerable<Invoice> GetYearToDateSales();
        IEnumerable<Invoice> GetInvoicesByCustomer(int customerId);
        IEnumerable<LineItem> GetLineItems(int invoiceId, int startingIndex, int roundToNearest);
        IEnumerable<LineItem> GetLineItems(int? itemId = 0, int? customerId = null, bool includeCustomer = true);
        IEnumerable<Item> GetItemTypes();
        void SaveLineItems(List<LineItem> lines, int invoiceId);
        Invoice GetInvoice(int invoiceId);
        int SaveInvoice(Invoice invoice);
        bool MarkInvoicePaid(int invoiceId);
        string SaveAttachmentsToInvoice(int invoiceId, List<IFormFile> files, bool customerCanSee);
    }
}
