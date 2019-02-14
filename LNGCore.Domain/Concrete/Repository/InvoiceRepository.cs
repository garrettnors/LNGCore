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

        public IInvoice GetInvoice(int invoiceId)
        {
            return _dbContext.Invoice.FirstOrDefault(f => f.Id == invoiceId) ?? new Invoice();
            //return _db.Query<Invoice>("select * from invoice where id = @invoiceId", new { invoiceId }).SingleOrDefault();
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
