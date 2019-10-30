using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface IPriceListService : IBaseService<PriceList>
    {
        List<PriceList> GetAll(string searchTerm = null);
    }
}
