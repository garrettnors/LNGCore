using LNGCore.Domain.Database;
using LNGCore.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNGCore.Domain.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly LngDbContext _db;
        public ItemService(LngDbContext db)
        {
            _db = db;
        }
        public int Add(Item item)
        {
            _db.Item.Add(item);
            _db.SaveChanges();

            return item.ItemId;
        }

        public void Delete(int itemId)
        {
            var item = _db.Item.Find(itemId);

            if (item == null)
                return;

            _db.Item.Remove(item);
            _db.SaveChanges();
        }

        public void Edit(Item item)
        {
            var dbItem = _db.Item.Find(item.ItemId);
            if (dbItem == null)
                return;

            _db.Entry(dbItem).CurrentValues.SetValues(item);
            _db.SaveChanges();
        }

        public Item Get(int itemId)
        {
            return _db.Item.Include(i => i.LineItem).FirstOrDefault(f => f.ItemId == itemId) ?? new Item();
        }

        public List<Item> GetAll(string searchTerm = null)
        {
            if (searchTerm != null)
            {
                searchTerm = searchTerm.ToLower();
                return _db.Item.Include(i => i.LineItem).Where(w => w.ItemName.ToLower().Contains(searchTerm)).ToList();
            }

            return _db.Item.Include(i => i.LineItem).ToList();
        }
    }
}
