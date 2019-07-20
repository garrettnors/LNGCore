using LNGCore.Domain.Database;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using static LNGCore.Domain.Infrastructure.Enums;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface IInvoiceService : IBaseService<Invoice>
    {
        IEnumerable<Invoice> GetInvoices(InvoiceTypeEnum type, string searchTerm = "");
        IEnumerable<Invoice> GetYearToDateSales();
        IEnumerable<Invoice> GetInvoicesByCustomer(int customerId);
        IEnumerable<LineItem> GetLineItems(int invoiceId, int startingIndex, int roundToNearest);
        IEnumerable<LineItem> GetLineItems(int? itemId = 0, int? customerId = null, bool includeCustomer = true);
        IEnumerable<Item> GetItemTypes();
        void SaveLineItems(List<LineItem> lines, int invoiceId);
        bool MarkInvoicePaid(int invoiceId);
        string SaveAttachmentsToInvoice(int invoiceId, List<IFormFile> files, bool customerCanSee);
    }
}
