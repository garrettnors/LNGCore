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

        public Employee Get(int employeeId)
        {
            return _db.Employee.FirstOrDefault(f => f.EmployeeId == employeeId) ?? new Employee();
        }

        public int Add(Employee employee)
        {
            _db.Employee.Add(employee);
            _db.SaveChanges();
            return employee.EmployeeId;
        }

        public void Edit(Employee employee)
        {
            var item = _db.Employee.Find(employee.EmployeeId);
            if (item == null)
                return;

            _db.Entry(item).CurrentValues.SetValues(employee);
            _db.SaveChanges();
        }

        public void Delete(int employeeId)
        {
            var item = _db.Employee.Find(employeeId);

            if (item == null)
                return;

            _db.Remove(item);
            _db.SaveChanges();
        }
    }
}
