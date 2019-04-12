using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly LngDbContext _db;
        public EmployeeService(LngDbContext context)
        {
            _db = context;
        }
        public IEnumerable<Employee> GetEmployees()
        {
            return _db.Employee;
        }

        public Employee GetEmployee(int employeeId)
        {
            return _db.Employee.FirstOrDefault(f => f.EmployeeId == employeeId) ?? new Employee();
        }
    }
}
