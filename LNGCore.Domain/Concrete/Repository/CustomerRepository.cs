using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.Domain.Concrete.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Domain.Abstract.Context;
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

        public IEnumerable<ICustomer> GetAllCustomers(string searchTerm = "")
        {
            var customers = db.Customer.Include(i => i.Invoice).ThenInclude(i => i.LineItem).ToList();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                customers = customers.Where(w =>
                    (w.BusinessName?.ToLower().Contains(searchTerm) ?? false) ||
                    (w.Name?.ToLower().Contains(searchTerm) ?? false) ||
                    (w.BusinessPhone?.ToLower().Contains(searchTerm) ?? false) ||
                    (w.AltPhone?.ToLower().Contains(searchTerm) ?? false) ||
                    (w.PostBox?.ToLower().Contains(searchTerm) ?? false) ||
                    (w.City?.ToLower().Contains(searchTerm) ?? false) ||
                    (w.State?.ToLower().Contains(searchTerm) ?? false) ||
                    (w.Street?.ToLower().Contains(searchTerm) ?? false) ||
                    (w.ZipCode?.ToLower().Contains(searchTerm) ?? false) ||
                    (w.Email?.ToLower().Contains(searchTerm) ?? false) ||
                    (w.TaxId?.ToLower().Contains(searchTerm) ?? false)
                ).ToList();
            }

            return customers;
        }

        public virtual ICustomer GetCustomer(int customerId)
        {
            return db.Customer.FirstOrDefault(f => f.Id == customerId) ?? new Customer();
        }

        public int SaveCustomer(ICustomer customer)
        {
            var saveCustomer = db.Customer.FirstOrDefault(f => f.Id == customer.Id) ?? new Customer();
            saveCustomer.BusinessName = customer.BusinessName;
            saveCustomer.Name = customer.Name;
            saveCustomer.BusinessPhone = customer.BusinessPhone;
            saveCustomer.AltPhone = customer.AltPhone;
            saveCustomer.PostBox = customer.PostBox;
            saveCustomer.City = customer.City;
            saveCustomer.State = customer.State;
            saveCustomer.Street = customer.Street;
            saveCustomer.ZipCode = customer.ZipCode;
            saveCustomer.Email = customer.Email;
            saveCustomer.TaxId = customer.TaxId;

            if (saveCustomer.Id == 0)
                db.Customer.Add(saveCustomer);

            db.Commit();
            return saveCustomer.Id;
        }
    }
}