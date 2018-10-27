using LNGCore.Domain.Abstract.Class;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Abstract.Repository
{
    public interface IBillSheetRepository
    {
        IEnumerable<IBillSheet> GetAllBills();
    }
}
