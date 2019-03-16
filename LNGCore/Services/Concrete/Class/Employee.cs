using System;
using System.Collections.Generic;
using System.Text;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.Services.Concrete.Class
{
    public class Employee : IEmployee
    {
        public int EmployeeID { get; set; }
        public string EmpName { get; set; }
    }
}
