using System;
using System.Collections.Generic;
using System.Text;
using LNGCore.Domain.Abstract.Class;

namespace LNGCore.Domain.Concrete.Class
{
    public class Employee : IEmployee
    {
        public int EmployeeID { get; set; }
        public string EmpName { get; set; }
    }
}
