using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAllCustomers(string searchTerm = "");
        Customer GetCustomer(int Id);
        int SaveCustomer(Customer customer);
    }
}
