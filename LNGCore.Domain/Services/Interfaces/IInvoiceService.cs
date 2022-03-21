using LNGCore.Domain.Database;
using LNGCore.UI.Models.Admin;
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
        void SetInvoiceStatus(int invoiceId, InvoiceTypeEnum status, string stripeChargeId = null);
        void SaveAttachmentsToInvoice(int invoiceId, List<IFormFile> files, bool customerCanSee);
        Dictionary<int, int> GetEmailCountsForInvoices(List<int> invoiceIds);
        int? GetNextInvoiceId(int invoiceId, InvoiceTypeEnum type);
        int? GetPreviousInvoiceId(int invoiceId, InvoiceTypeEnum type);
        void SetParticipantPaidStatus(List<int> ids, bool isPaid);
        Invoice GetByIdentifierGuid(string guid);
        byte[] GetInvoicePdfBytes(int invoiceId);
    }
}
