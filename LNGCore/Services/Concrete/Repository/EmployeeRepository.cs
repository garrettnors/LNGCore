using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Services.Abstract.Class;
using LNGCore.Services.Abstract.Context;
using LNGCore.Services.Abstract.Repository;
using LNGCore.Services.Concrete.Class;

namespace LNGCore.Services.Concrete.Repository
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
