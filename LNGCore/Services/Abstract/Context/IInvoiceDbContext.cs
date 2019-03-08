using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Concrete.Class;
using Microsoft.EntityFrameworkCore;

namespace LNGCore.Domain.Abstract.Context
{
    public interface IInvoiceDbContext
    {
        DbSet<BillSheet> BillSheet { get; set; }
        DbSet<Customer> Customer { get; set; }
        DbSet<Invoice> Invoice { get; set; }
        DbSet<Event> Event { get; set; }
        DbSet<Item> Item { get; set; }
        DbSet<Employee> Employee { get; set; }
        DbSet<LineItem> LineItem { get; set; }
        DbSet<OrnamentOrders> OrnamentOrders { get; set; }
        DbSet<InvoiceAttachment> InvoiceAttachments { get; set; }
        DbSet<Log> Log { get; set; }
        void Commit();
    }
}