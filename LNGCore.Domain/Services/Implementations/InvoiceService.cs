using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Services.Implementations
{
    public class InvoiceService : IInvoiceService
    {
        private readonly LngDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;
        public InvoiceService(LngDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _db = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IEnumerable<Invoice> GetDonatedItems(string searchTerm = "")
        {
            var items = _db.Invoice.Where(w => w.IsDonated == true && !w.Voided);

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .OrderByDescending(o => o.OrderDate);
        }

        public Invoice GetInvoice(int invoiceId)
        {
            if (invoiceId == 0)
                return new Invoice();

            return _db.Invoice
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .FirstOrDefault(f => f.Id == invoiceId);
        }

        public IEnumerable<Invoice> GetInvoicesByCustomer(int customerId)
        {
            return _db.Invoice.Where(w => w.CustomerId == customerId)
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .OrderByDescending(o => o.OrderDate);
        }

        public IEnumerable<Item> GetItemTypes()
        {
            return _db.Item.OrderBy(o => o.ItemName);
        }
        public IEnumerable<LineItem> GetLineItems(int invoiceId, int startingIndex, int roundToNearest)
        {
            var existingLines = _db.LineItem.Where(w => w.InvoiceId == invoiceId).ToList();

            if (existingLines.Count == 0)
                existingLines.Add(new LineItem());

            while (existingLines.Count % roundToNearest != 0)
                existingLines.Add(new LineItem());

            return existingLines;
        }

        public IEnumerable<LineItem> GetLineItems(int? itemId = null, int? customerId = null, bool includeCustomer = true)
        {
            IQueryable<LineItem> lineItems = _db.LineItem;

            if (itemId != null)
                lineItems = lineItems.Where(w => w.ItemId == itemId);

            if (customerId != null)
            {
                lineItems = includeCustomer
                   ? lineItems.Where(w => w.Invoice.CustomerId == customerId)
                   : lineItems.Where(w => w.Invoice.CustomerId != customerId);
            }

            return lineItems.OrderByDescending(o => o.Invoice.OrderDate).Take(40).Include(i => i.Invoice).ThenInclude(t => t.Customer).Include(i => i.Item);
        }

        public IEnumerable<Invoice> GetOpenInvoices(string searchTerm = "")
        {
            var items = _db.Invoice.Where(
               w => w.IsDonated == false &&
                    !w.IsQuote && !w.Voided &&
                    w.IsPaid == false);

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .OrderByDescending(o => o.OrderDate);
        }

        public IEnumerable<Invoice> GetOpenQuotes(string searchTerm = "")
        {
            var items = _db.Invoice.Where(w => w.IsQuote && !w.Voided);

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .OrderByDescending(o => o.OrderDate);
        }

        public IEnumerable<Invoice> GetPaidInvoices(string searchTerm = "")
        {
            var items = _db.Invoice.Where(w => w.IsPaid == true && !w.Voided);

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .OrderByDescending(o => o.OrderDate);
        }

        public IEnumerable<Invoice> GetPastDueInvoices(string searchTerm = "")
        {
            var items = _db.Invoice.Where(
                w => w.IsDonated == false &&
                     !w.IsQuote &&
                     !w.Voided &&
                     w.IsPaid == false &&
                     DateTime.Now >= w.OrderDate.AddMonths(1));

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .OrderByDescending(o => o.OrderDate);
        }

        public IEnumerable<Invoice> GetVoidedItems(string searchTerm = "")
        {
            var items = _db.Invoice.Where(w => w.Voided);

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .OrderByDescending(o => o.OrderDate);
        }

        public IEnumerable<Invoice> GetYearToDateSales()
        {
            return _db.Invoice.Where(
                    w => w.IsDonated == false &&
                         !w.IsQuote &&
                         !w.Voided &&
                         w.OrderDate.Year == DateTime.Now.Year)
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .OrderBy(o => o.OrderDate);
        }

        public bool MarkInvoicePaid(int invoiceId)
        {
            var invoice = _db.Invoice.FirstOrDefault(f => f.Id == invoiceId);

            if (invoice == null)
                return false;

            //invoice.IsPaid = true;
            //invoice.PaidDate = DateTime.Now;
            //_db.SaveChanges();
            //todo: re-enable this

            return true;
        }

        public int SaveInvoice(Invoice invoice)
        {
            var saveItem = _db.Invoice.FirstOrDefault(f => f.Id == invoice.Id) ?? new Invoice();

            saveItem.CustomerId = invoice.CustomerId;
            saveItem.OrderDate = invoice.OrderDate;
            saveItem.CompletedBy = invoice.CompletedBy;
            saveItem.Deadline = invoice.Deadline;
            saveItem.EmployeeId = invoice.EmployeeId;
            saveItem.InvoiceProofUrl = invoice.InvoiceProofUrl;
            saveItem.IsDonated = invoice.IsDonated;
            saveItem.IsPaid = invoice.IsPaid;
            saveItem.PaidDate = invoice.PaidDate;
            saveItem.IsQuote = invoice.IsQuote;
            saveItem.Notes = invoice.Notes;
            saveItem.Pofield = invoice.Pofield;
            saveItem.Voided = invoice.Voided;
            saveItem.ShippingMethod = invoice.ShippingMethod;
            saveItem.ShipCost = invoice.ShipCost;
            saveItem.TaxPercent = (decimal)8.2500;

            if (saveItem.Id == 0)
                _db.Invoice.Add(saveItem);

            _db.SaveChanges();

            return saveItem.Id;
        }

        public void SaveLineItems(List<LineItem> lines, int invoiceId)
        {
            foreach (var lineItem in lines)
            {
                var line = _db.LineItem.FirstOrDefault(f => f.LineItemId == lineItem.LineItemId) ??
                           new LineItem();
                line.InvoiceId = invoiceId;
                line.ItemDesc = lineItem.ItemDesc;
                line.ItemId = lineItem.ItemId;
                line.ItemPrice = lineItem.ItemPrice;
                line.Quantity = lineItem.Quantity;
                line.Price = lineItem.Price;

                if (line.LineItemId == 0)
                    _db.LineItem.Add(line);
            }

            _db.SaveChanges();
        }
        public string SaveAttachmentsToInvoice(int invoiceId, List<IFormFile> files, bool customerCanSee)
        {

            foreach (var file in files)
            {
                var fileName = $"Invoice_{invoiceId}_{Guid.NewGuid().ToString().Substring(0, 6)}{Path.GetExtension(file.FileName)}";

                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, fileName);

                using (var fileStream = File.Create(filePath))
                {
                    file.CopyTo(fileStream);
                }

                //to do : Save uniqueFileName  to your db table   
            }
            return string.Empty;
        }
        private IQueryable<Invoice> SearchItems(IQueryable<Invoice> items, string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                if (DateTime.TryParse(searchTerm, out var searchedDate))
                {
                    items = items.Where(w =>
                        w.OrderDate == searchedDate || w.PaidDate == searchedDate || w.Deadline == searchedDate);
                }

                items = items.Where(w =>
                    w.Customer.Name.ToLower().Contains(searchTerm) ||
                    w.Customer.BusinessName.ToLower().Contains(searchTerm) ||
                    w.Notes.ToLower().Contains(searchTerm) ||
                    w.LineItem.Any(a => a.ItemDesc.ToLower().Contains(searchTerm)));
            }

            return items;
        }
    }
}
