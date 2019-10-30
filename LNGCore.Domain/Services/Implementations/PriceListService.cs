using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Services.Implementations
{
    public class PriceListService : IPriceListService
    {
        private readonly LngDbContext _db;
        public PriceListService(LngDbContext context)
        {
            _db = context;
        }
        public int Add(PriceList item)
        {
            _db.PriceList.Add(item);
            _db.SaveChanges();
            return item.Id;
        }

        public void Edit(PriceList priceList)
        {
            var item = _db.PriceList.Find(priceList.Id);
            if (item == null)
                return;

            _db.Entry(item).CurrentValues.SetValues(priceList);
            _db.SaveChanges();
        }

        public void Delete(int priceListId)
        {
            var item = _db.PriceList.Find(priceListId);

            if (item == null)
                return;

            _db.Remove(item);
            _db.SaveChanges();
        }

        public PriceList Get(int itemId)
        {
            return _db.PriceList.FirstOrDefault(f => f.Id == itemId) ?? new PriceList();
        }

        public List<PriceList> GetAll(string searchTerm = null)
        {
            if (searchTerm != null)
            {
                searchTerm = searchTerm.ToLower();
                return _db.PriceList.Where(w => 
                (w.ItemType == null ? false : w.ItemType.Contains(searchTerm))
                || (w.ItemNumber == null ? false : w.ItemNumber.Contains(searchTerm))
                || (w.ItemDesc == null ? false : w.ItemDesc.Contains(searchTerm))
                ).ToList();
            }

            return _db.PriceList.ToList();
        }
    }
}
