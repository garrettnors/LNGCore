﻿
using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface IBillSheetService
    {
        IEnumerable<BillSheet> GetAllBills();
    }
}
