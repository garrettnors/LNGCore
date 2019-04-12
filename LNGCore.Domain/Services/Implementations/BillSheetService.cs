using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNGCore.Domain.Services.Implementations
{

    public class BillSheetService : IBillSheetService
    {
        private readonly LngDbContext _db;
        public BillSheetService(LngDbContext context)
        {
            _db = context;
        }
        public IEnumerable<BillSheet> GetAllBills()
        {
            return _db.BillSheet;
        }
    }
}
