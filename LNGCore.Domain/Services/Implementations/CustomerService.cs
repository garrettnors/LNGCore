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

        public virtual Customer GetCustomer(int customerId)
        {
            return _db.Customer.FirstOrDefault(f => f.Id == customerId) ?? new Customer();
        }

        public int SaveCustomer(Customer customer)
        {
            var saveCustomer = _db.Customer.FirstOrDefault(f => f.Id == customer.Id) ?? new Customer();
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
            saveCustomer.SecondaryEmail = customer.SecondaryEmail;
            saveCustomer.TaxId = customer.TaxId;

            if (saveCustomer.Id == 0)
                _db.Customer.Add(saveCustomer);

            _db.SaveChanges();
            return saveCustomer.Id;
        }
    }
}
