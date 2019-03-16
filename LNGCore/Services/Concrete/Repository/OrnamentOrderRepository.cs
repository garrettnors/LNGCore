using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Services.Abstract.Class;
using LNGCore.Services.Abstract.Context;
using LNGCore.Services.Abstract.Repository;
using LNGCore.Services.Concrete.Class;
using LNGCore.Services.Concrete.Context;

namespace LNGCore.Services.Concrete.Repository
{
    public class OrnamentOrderRepository : IOrnamentOrderRepository
    {
        private readonly IInvoiceDbContext db;
        public OrnamentOrderRepository(IInvoiceDbContext dbContext)
        {
            db = dbContext;
        }
        public int GetRemainingOrnamentStockByStyle(string styleName)
        {
            var maxOrnamentCount = 200;
            return maxOrnamentCount - db.OrnamentOrders.Count(w => w.OrnamentStyle == styleName);
        }

        public void SaveOrnamentOrder(List<IOrnamentOrders> ornamentOrders)
        {
            db.OrnamentOrders.AddRange(ornamentOrders.Select(s => new OrnamentOrders { Amount = s.Amount, SpecialInstructions = s.SpecialInstructions, OrnamentDesign = s.OrnamentDesign, OrnamentStyle = s.OrnamentStyle, UserEmail = s.UserEmail, UserName = s.UserName }));
            db.Commit();
        }
    }
}
