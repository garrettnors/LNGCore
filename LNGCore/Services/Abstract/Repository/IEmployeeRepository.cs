using System;
using System.Collections.Generic;
using System.Text;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.Services.Abstract.Repository
{
    public interface IEmployeeRepository
    {
        IEnumerable<IEmployee> GetEmployees();
        IEmployee GetEmployee(int employeeId);
    }
}
