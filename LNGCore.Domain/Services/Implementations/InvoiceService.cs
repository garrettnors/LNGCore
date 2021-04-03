using LNGCore.Domain.Database;
using LNGCore.Domain.Infrastructure;
using LNGCore.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static LNGCore.Domain.Infrastructure.Enums;

namespace LNGCore.Domain.Services.Implementations
{
    public class InvoiceService : IInvoiceService
    {
        private readonly LngDbContext _db;
        private readonly ICustomerService _customerService;
        private readonly ILogService _logService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public InvoiceService(LngDbContext context, ICustomerService customerService, ILogService logService, IHostingEnvironment hostingEnvironment)
        {
            _db = context;
            _customerService = customerService;
            _logService = logService;
            _hostingEnvironment = hostingEnvironment;
        }

        public Invoice Get(int invoiceId)
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

        public void Delete(int itemId)
        {
            var invoice = _db.Invoice.Find(itemId);

            if (invoice == null)
                return;

            invoice.Voided = true;
            _db.SaveChanges();
        }

        public void Edit(Invoice item)
        {
            var invoice = Get(item.Id);

            if (invoice == null)
                return;

            item.Identifier = invoice.Identifier;
            

            _db.Entry(invoice).CurrentValues.SetValues(item);
            _db.SaveChanges();
        }

        public int Add(Invoice invoice)
        {
            var customer = _customerService.Get(invoice.CustomerId);

            invoice.TaxPercent = (decimal)8.2500;
            invoice.Identifier = Guid.NewGuid().ToString();

            _db.Invoice.Add(invoice);
            _db.SaveChanges();

            return invoice.Id;
        }

        public IEnumerable<Invoice> GetInvoices(InvoiceTypeEnum type, string searchTerm = "")
        {
            IQueryable<Invoice> items = _db.Invoice;
            switch (type)
            {
                case InvoiceTypeEnum.Open:
                    items = items.Where(
                        w => w.IsDonated == false &&
                        !w.IsQuote && !w.Voided &&
                        w.IsPaid == false);
                    break;
                case InvoiceTypeEnum.Paid:
                    items = items.Where(w => w.IsPaid == true && !w.Voided);
                    break;
                case InvoiceTypeEnum.Quote:
                    items = items.Where(w => w.IsQuote && !w.Voided);
                    break;
                case InvoiceTypeEnum.Voided:
                    items = items.Where(w => w.Voided);
                    break;
                case InvoiceTypeEnum.Donated:
                    items = items.Where(w => w.IsDonated == true && !w.Voided);
                    break;
                case InvoiceTypeEnum.PastDue:
                    items = items.Where(
                        w => w.IsDonated == false &&
                        !w.IsQuote &&
                        !w.Voided &&
                         w.IsPaid == false &&
                         DateTime.Now >= w.OrderDate.AddMonths(1));
                    break;
                case InvoiceTypeEnum.All:
                default:
                    break;
            }

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .OrderBy(o => o.IsPaidToEmployees).ThenByDescending(t => t.OrderDate);
        }

        public IEnumerable<Invoice> GetInvoicesByCustomer(int customerId)
        {
            return GetInvoices(InvoiceTypeEnum.All).Where(w => w.CustomerId == customerId);
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



        public void SaveLineItems(List<LineItem> lines, int invoiceId)
        {
            //remove all existing lineitems and add new lines (in case of edits where lines were removed)
            var existingItems = _db.LineItem.Where(w => w.InvoiceId == invoiceId);
            _db.LineItem.RemoveRange(existingItems);
            
            var invoice = Get(invoiceId);
            var customerTaxRate = invoice.Customer.Taxable ? invoice.TaxPercent / 100 : 0;
            
            foreach (var lineItem in lines)
            {
                var line = new LineItem
                {
                    InvoiceId = invoiceId,
                    ItemDesc = lineItem.ItemDesc,
                    ItemId = lineItem.ItemId,
                    ItemPrice = lineItem.ItemPrice,
                    Quantity = lineItem.Quantity,
                    TaxAmount = (lineItem.ItemPrice ?? 0) * customerTaxRate
                };


                _db.LineItem.Add(line);
            }

            _db.SaveChanges();
        }
        public void SaveAttachmentsToInvoice(int invoiceId, List<IFormFile> files, bool customerCanSee)
        {
            foreach (var file in files)
            {
                var fileName = $"Invoice_{invoiceId}_{Guid.NewGuid().ToString().Substring(0, 6)}{Path.GetExtension(file.FileName)}";

                var uploads = Path.Combine("Uploads", $"{invoiceId}");
                Directory.CreateDirectory(uploads);
                var filePath = Path.Combine(uploads, fileName);

                using (var fileStream = File.Create(filePath))
                {
                    file.CopyTo(fileStream);
                }
            }
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

        public void SetInvoiceStatus(int invoiceId, InvoiceTypeEnum status, string stripeChargeId = null)
        {
            var invoice = Get(invoiceId);

            if (invoice?.Id == 0)
                throw new InvalidDataException();

            invoice.StripeChargeId = stripeChargeId;
            invoice.IsPaid = false;
            invoice.Voided = false;
            invoice.IsQuote = false;
            invoice.IsDonated = false;
            invoice.PaidDate = null;
            switch (status)
            {
                case InvoiceTypeEnum.Paid:
                    invoice.IsPaid = true;
                    invoice.PaidDate = DateTime.Now;
                    break;
                case InvoiceTypeEnum.Quote:
                    invoice.IsQuote = true;
                    break;
                case InvoiceTypeEnum.Voided:
                    invoice.Voided = true;
                    break;
                case InvoiceTypeEnum.Donated:
                    invoice.IsDonated = true;
                    break;
                default:
                    break;
            }
            _db.SaveChanges();
        }

        public Dictionary<int, int> GetEmailCountsForInvoices(List<int> invoiceIds)
        {
            var logs = _logService.GetLogsByInvoiceId(invoiceIds).ToList();
            var returnDict = new Dictionary<int, int>();

            foreach (var item in invoiceIds)
                returnDict.Add(item, logs.Count(c => c.InvoiceId == item));

            return returnDict;
        }

        public int? GetNextInvoiceId(int invoiceId, InvoiceTypeEnum type)
        {
            var currentInvc = Get(invoiceId);

            if (currentInvc == null)
                return null;

            return GetInvoices(type).Where(w => w.Id > invoiceId)?.OrderBy(o => o.Id).FirstOrDefault()?.Id;
        }

        public int? GetPreviousInvoiceId(int invoiceId, InvoiceTypeEnum type)
        {
            var currentInvc = Get(invoiceId);

            if (currentInvc == null)
                return null;

            return GetInvoices(type).Where(w => w.Id < invoiceId)?.OrderBy(o => o.Id).LastOrDefault()?.Id;
        }

        public void SetParticipantPaidStatus(List<int> ids, bool isPaid)
        {
            foreach (var id in ids)
                _db.Invoice.FirstOrDefault(f => f.Id == id).IsPaidToEmployees = isPaid;

            _db.SaveChanges();
        }

        public Invoice GetByIdentifierGuid(string guid)
        {
            return _db.Invoice
                .Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.LineItem)
                .ThenInclude(t => t.Item)
                .FirstOrDefault(f => f.Identifier == guid);
        }
    }
}