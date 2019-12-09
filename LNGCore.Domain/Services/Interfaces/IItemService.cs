using LNGCore.Domain.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface IItemService : IBaseService<Item>
    {
        List<Item> GetAll(string searchTerm = null);
    }
}
