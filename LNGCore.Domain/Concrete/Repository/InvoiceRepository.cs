using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.Domain.Concrete.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LNGCore.Domain.Concrete.Class;
using Microsoft.EntityFrameworkCore;

namespace LNGCore.Domain.Concrete.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IInvoiceDbContext db;
        public InvoiceRepository(IInvoiceDbContext dbContext)
        {
            db = dbContext;
        }

        public virtual IInvoice GetInvoice(int invoiceId)
        {
            return db.Invoice.FirstOrDefault(f => f.Id == invoiceId) ?? new Invoice();
        }

        public IEnumerable<IInvoice> GetInvoices()
        {
            return db.Invoice.Include(i => i.Customer).Include(i => i.LineItem);
        }

        public virtual IEnumerable<IInvoice> GetInvoicesByCustomer(int customerId)
        {
            return db.Invoice.Where(w => w.CustomerId == customerId).Include(i => i.Customer).Include(i => i.LineItem);
        }
    }
}
