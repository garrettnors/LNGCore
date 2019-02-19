using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.Domain.Concrete.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using Dapper;
using LNGCore.Domain.Abstract.Context;
using LNGCore.Domain.Concrete.Class;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LNGCore.Domain.Concrete.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _db;
        private readonly IInvoiceDbContext _dbContext;

        public InvoiceRepository(IConfiguration configuration, IInvoiceDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _db = new SqlConnection(_configuration.GetSection("SiteConfiguration")["DbContext"]);
        }

        public IEnumerable<ILineItem> GetLineItems(int invoiceId, int startingIndex, int roundToNearest)
        {
            var existingLines = _dbContext.LineItem.Where(w => w.InvoiceId == invoiceId).ToList();

            if (existingLines.Count == 0)
                existingLines.Add(new LineItem());

            while (existingLines.Count % roundToNearest != 0)
                existingLines.Add(new LineItem());

            return existingLines;
        }

        public IEnumerable<ILineItem> GetLineItems(string searchTerm, int? customerId = 0)
        {
            searchTerm = searchTerm?.ToLower();

            if (customerId == 0)
            {
                return _dbContext.LineItem.Where(w =>
                        w.ItemDesc.ToLower().Contains(searchTerm) ||
                        w.Item.ItemName.ToLower().Contains(searchTerm))
                    .OrderByDescending(o => o.Invoice.OrderDate).Take(10).Include(i => i.Invoice)
                    .ThenInclude(t => t.Customer);
            }

            return _dbContext.LineItem.Where(w =>
                    w.Invoice.CustomerId == customerId && w.ItemDesc.ToLower().Contains(searchTerm) ||
                    w.Item.ItemName.ToLower().Contains(searchTerm)).OrderByDescending(o => o.Invoice.OrderDate).Take(10)
                .Include(i => i.Invoice).ThenInclude(t => t.Customer).Include(i => i.Item);
        }

        public IEnumerable<IItem> GetItemTypes()
        {
            return _dbContext.Item.OrderBy(o => o.ItemName);
        }

        public IInvoice GetInvoice(int invoiceId)
        {
            return _dbContext.Invoice.FirstOrDefault(f => f.Id == invoiceId) ?? new Invoice();
            //return _db.Query<Invoice>("select * from invoice where id = @invoiceId", new { invoiceId }).SingleOrDefault();
        }

        public int SaveInvoice(IInvoice invoice)
        {
            var saveItem = _dbContext.Invoice.FirstOrDefault(f => f.Id == invoice.Id) ?? new Invoice();

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
                _dbContext.Invoice.Add(saveItem);

            _dbContext.Commit();

            return saveItem.Id;
        }

        public void SaveLineItems(List<ILineItem> lines, int invoiceId)
        {
            foreach (var lineItem in lines)
            {
                var line = _dbContext.LineItem.FirstOrDefault(f => f.LineItemId == lineItem.LineItemId) ??
                           new LineItem();
                line.InvoiceId = invoiceId;
                line.ItemDesc = lineItem.ItemDesc;
                line.ItemId = lineItem.ItemId;
                line.ItemPrice = lineItem.ItemPrice;
                line.Quantity = lineItem.Quantity;
                line.Price = lineItem.Price;

                if (line.LineItemId == 0)
                    _dbContext.LineItem.Add(line);
            }

            _dbContext.Commit();
        }

        public bool MarkInvoicePaid(int invoiceId)
        {
            var invoice = _dbContext.Invoice.FirstOrDefault(f => f.Id == invoiceId);

            if (invoice == null)
                return false;

            //invoice.IsPaid = true;
            //invoice.PaidDate = DateTime.Now;
            //_dbContext.Commit();

            return true;
        }

        public IEnumerable<IInvoice> GetYearToDateSales()
        {
            return _dbContext.Invoice.Where(
                    w => w.IsDonated == false &&
                         !w.IsQuote &&
                         !w.Voided &&
                         w.OrderDate.Year == DateTime.Now.Year)
                .Include(i => i.LineItem)
                .Include(i => i.Customer)
                .OrderBy(o => o.OrderDate);
            //return _db.Query<Invoice>(
            //    @"select *, (select sum(ItemPrice*Quantity) from LineItem where LineItem.InvoiceId = Invoice.ID) as InvoiceTotal from Invoice                  
            //      where  YEAR(GETDATE()) = YEAR(OrderDate)
            //      and invoice.isDonated = 0
            //      and invoice.Voided = 0
            //      and invoice.IsQuote = 0");
        }

        public IEnumerable<IInvoice> GetOpenInvoices(string searchTerm = "")
        {
            var items = _dbContext.Invoice.Where(
                w => w.IsDonated == false &&
                     !w.IsQuote && !w.Voided &&
                     w.IsPaid == false);

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.LineItem)
                .Include(i => i.Customer)
                .OrderByDescending(o => o.OrderDate);
            //return _db.Query<Invoice>(
            //    @"select *, (select sum(ItemPrice*Quantity) from LineItem where LineItem.InvoiceId = Invoice.ID) as InvoiceTotal from Invoice                  
            //      where 
            //      invoice.isPaid = 0
            //      and invoice.isDonated = 0
            //      and invoice.Voided = 0
            //      and invoice.IsQuote = 0");
        }

        public IEnumerable<IInvoice> GetPastDueInvoices(string searchTerm = "")
        {
            var items = _dbContext.Invoice.Where(
                w => w.IsDonated == false &&
                     !w.IsQuote &&
                     !w.Voided &&
                     w.IsPaid == false &&
                     DateTime.Now >= w.OrderDate.AddMonths(1));

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.LineItem)
                .Include(i => i.Customer)
                .OrderByDescending(o => o.OrderDate);
            //return _db.Query<Invoice>(
            //    @"select *, (select sum(ItemPrice*Quantity) from LineItem where LineItem.InvoiceId = Invoice.ID) as InvoiceTotal from Invoice
            //      where  GETDATE() >= DATEADD(month,1,Invoice.OrderDate)
            //      and invoice.isPaid = 0
            //      and invoice.isDonated = 0
            //      and invoice.Voided = 0
            //      and invoice.IsQuote = 0");
        }

        public IEnumerable<IInvoice> GetPaidInvoices(string searchTerm = "")
        {
            var items = _dbContext.Invoice.Where(w => w.IsPaid == true && !w.Voided);

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.LineItem)
                .Include(i => i.Customer)
                .OrderByDescending(o => o.OrderDate);
            //return _db.Query<Invoice>("select * from invoice where isPaid = 1 and voided = 0");
        }

        public IEnumerable<IInvoice> GetOpenQuotes(string searchTerm = "")
        {
            var items = _dbContext.Invoice.Where(w => w.IsQuote && !w.Voided);

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.LineItem)
                .Include(i => i.Customer)
                .OrderByDescending(o => o.OrderDate);
            //return _db.Query<Invoice>("select * from invoice where isQuote = 1 and voided = 0");
        }

        public IEnumerable<IInvoice> GetDonatedItems(string searchTerm = "")
        {
            var items = _dbContext.Invoice.Where(w => w.IsDonated == true && !w.Voided);

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.LineItem)
                .Include(i => i.Customer)
                .OrderByDescending(o => o.OrderDate);
            //return _db.Query<Invoice>("select * from invoice where isDonated = 1 and voided = 0");
        }

        public IEnumerable<IInvoice> GetVoidedItems(string searchTerm = "")
        {
            var items = _dbContext.Invoice.Where(w => w.Voided);

            items = SearchItems(items, searchTerm);

            return items
                .Include(i => i.LineItem)
                .Include(i => i.Customer)
                .OrderByDescending(o => o.OrderDate);
            //return _db.Query<Invoice>("select * from invoice where voided = 1");
        }

        public IEnumerable<IInvoice> GetInvoicesByCustomer(int customerId)
        {
            return _dbContext.Invoice.Where(w => w.CustomerId == customerId)
                .Include(i => i.LineItem)
                .Include(i => i.Customer)
                .OrderByDescending(o => o.OrderDate);
            //return _db.Query<Invoice>("select * from invoice where customerId = @customerId", new { customerId });
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