using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LNGCore.Services.Abstract.Class
{
    public interface IEmployee
    {
        int EmployeeID { get; set; }
        [MaxLength(50)]
        string EmpName { get; set; }
    }
}
