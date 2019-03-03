using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Context;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.Domain.Concrete.Class;

namespace LNGCore.Domain.Concrete.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IInvoiceDbContext _db;
        public EmployeeRepository(IInvoiceDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<IEmployee> GetEmployees()
        {
            return _db.Employee;
        }

        public IEmployee GetEmployee(int employeeId)
        {
            return _db.Employee.FirstOrDefault(f => f.EmployeeID == employeeId) ?? new Employee();
        }
    }
}
