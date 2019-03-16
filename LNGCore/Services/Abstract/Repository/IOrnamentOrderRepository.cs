using System;
using System.Collections.Generic;
using System.Text;
using LNGCore.Services.Abstract.Class;

namespace LNGCore.Services.Abstract.Repository
{
    public interface IOrnamentOrderRepository
    {
        int GetRemainingOrnamentStockByStyle(string styleName);
        void SaveOrnamentOrder(List<IOrnamentOrders> ornamentOrders);
    }
}
