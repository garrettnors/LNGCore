using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.Domain.Concrete.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Domain.Concrete.Class;

namespace LNGCore.Domain.Concrete.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IInvoiceDbContext db;
        public CustomerRepository(IInvoiceDbContext dbContext)
        {
            db = dbContext;
        }
        public virtual IEnumerable<ICustomer> GetAllCustomers()
        {
            return db.Customer.Include(i => i.Invoice).ThenInclude(i => i.LineItem);
        }

        public virtual ICustomer GetCustomer(int customerId)
        {
            return db.Customer.FirstOrDefault(f => f.Id == customerId) ?? new Customer();
        }
    }
}
