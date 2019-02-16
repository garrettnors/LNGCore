using System;
using System.Collections.Generic;
using System.Text;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.Domain.Abstract.Repository
{
    public interface IEmployeeRepository
    {
        IEnumerable<IEmployee> GetEmployees();
    }
}
