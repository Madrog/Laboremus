using OfflineStore.BL;
using OfflineStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfflineStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.DateRange = string.Empty;
            ViewBag.TotalProfit = string.Empty;
            ViewBag.Top5ItemTypes = new Dictionary<string, decimal>();
            return View();
        }

        [HttpPost]
        public ActionResult Index(DateTime date1, DateTime date2)
        {
            SalesBL salesBL = new SalesBL();
            decimal total_profit = 0.0M;
            Dictionary<string, decimal> top5ItemTypes = new Dictionary<string, decimal>();
           
            top5ItemTypes = salesBL.GetTop5ProfitableItemTypes(date1, date2);
            total_profit = salesBL.GetTotalProfitMade(date1, date2);
            ViewBag.DateRange = $"DateRange Results From: {date1.ToString("dd-MMM-yyyy")} To: {date2.ToString("dd-MMM-yyyy")}";
            ViewBag.Top5ItemTypes = top5ItemTypes;
            ViewBag.TotalProfit = total_profit.ToString("#,###.00");
            
            return View();
        }
    }
}