using LNGCore.Domain.Abstract.Class;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Abstract.Repository
{
    public interface ICustomerRepository
    {
        IEnumerable<ICustomer> GetAllCustomers(string searchTerm = "");
        ICustomer GetCustomer(int Id);
        int SaveCustomer(ICustomer customer);
    }
}
