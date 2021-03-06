﻿using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        IEnumerable<Employee> GetEmployees();
    }
}
