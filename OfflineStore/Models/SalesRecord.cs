using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfflineStore.Models
{
    public class SalesRecord
    {
        public string Region { get; set; }
        public string Country { get; set; }
        public string ItemType { get; set; }
        public string SalesChannel { get; set; }
        public char OrderPriority { get; set; }
        public DateTime OrderDate { get; set; }
        public long OrderID { get; set; }
        public DateTime ShipDate { get; set; }
        public long UnitsSold { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal UnitCost { get; set; }
        public Decimal TotalRevenue { get; set; }
        public Decimal TotalCost { get; set; }
        public Decimal TotalProfit { get; set; }
    }
}