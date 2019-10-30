using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LNGCore.Domain.Database
{
    public partial class PriceList
    {
        public PriceList()
        {           
        }

        public int Id { get; set; }
        [DisplayName("Item Type")]
        public string ItemType { get; set; }
        [DisplayName("Item Number")]
        public string ItemNumber { get; set; }
        [DisplayName("Item Description")]
        public string ItemDesc { get; set; }
        [DisplayName("Case Qty")]
        public int? CaseQty { get; set; }
        [DisplayName("Our Cost")]
        public decimal? OurCost { get; set; }
        [DisplayName("Our Bulk Cost")]
        public decimal? OurBulkCost { get; set; }
        [DisplayName("Retail Price")]
        public decimal? RetailPrice { get; set; }
        [DisplayName("Retail Bulk Price")]
        public decimal? RetailBulkPrice { get; set; }
    }
}
