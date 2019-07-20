using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Interfaces
{
    public interface IBaseService<T>
    {
        T Get(int itemId);
        void Delete(int itemId);
        void Edit(T item);
        int Add(T item);
    }
}
