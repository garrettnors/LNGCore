using LNGCore.Domain.Abstract.Class;
using LNGCore.Domain.Abstract.Repository;
using LNGCore.Domain.Concrete.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNGCore.Domain.Abstract.Context;

namespace LNGCore.Domain.Concrete.Repository
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
