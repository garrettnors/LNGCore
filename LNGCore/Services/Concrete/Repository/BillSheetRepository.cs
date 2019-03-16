using LNGCore.Services.Abstract.Class;
using LNGCore.Services.Abstract.Repository;
using LNGCore.Services.Concrete.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Services.Abstract.Context;

namespace LNGCore.Services.Concrete.Repository
{
    public class BillSheetRepository : IBillSheetRepository
    {
        private readonly IInvoiceDbContext db;
        public BillSheetRepository(IInvoiceDbContext dbContext)
        {
            db = dbContext;
        }

        public virtual IEnumerable<IBillSheet> GetAllBills()
        {
            return db.BillSheet;
        }
    }
}
