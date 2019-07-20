using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly LngDbContext _db;
        public CustomerService(LngDbContext context)
        {
            _db = context;
        }
        public IEnumerable<Customer> GetAllCustomers(string searchTerm = "")
        {
            var customers = _db.Customer.Include(i => i.Invoice).ThenInclude(i => i.LineItem).ToList();
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

        public virtual Customer Get(int customerId)
        {
            return _db.Customer.FirstOrDefault(f => f.Id == customerId) ?? new Customer();
        }

        public int Add(Customer customer)
        {                       
            _db.Customer.Add(customer);
            _db.SaveChanges();
            return customer.Id;
        }

        public void Edit(Customer customer)
        {
            var item = _db.Customer.Find(customer.Id);
            if (item == null)
                return;

            _db.Entry(item).CurrentValues.SetValues(customer);
            _db.SaveChanges();
        }

        public void Delete(int customerId)
        {
            var item = _db.Customer.Find(customerId);

            if (item == null)
                return;

            _db.Remove(item);
            _db.SaveChanges();
        }
    }
}
