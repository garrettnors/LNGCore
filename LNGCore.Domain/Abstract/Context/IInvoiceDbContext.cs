using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Concrete.Class;
using Microsoft.EntityFrameworkCore;

namespace LNGCore.Domain.Concrete.Context
{
    public interface IInvoiceDbContext
    {
        DbSet<BillSheet> BillSheet { get; set; }
        DbSet<Customer> Customer { get; set; }
        DbSet<Invoice> Invoice { get; set; }
        DbSet<Item> Item { get; set; }
        DbSet<LineItem> LineItem { get; set; }
        DbSet<OrnamentOrders> OrnamentOrders { get; set; }
        void Commit();
    }
}