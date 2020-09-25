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
    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult Index()
        {
            SalesBL salesBL = new SalesBL();
            List<SalesRecord> sales = salesBL.GetSalesRecords();
            return View(sales.Take(100));
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase postedFile)
        {
            List<SalesRecord> sales = new List<SalesRecord>();
            CsvImport csvImport = new CsvImport();

            string filePath = string.Empty;
            if(postedFile == null|| postedFile.ContentLength == 0)
            {
                ViewBag.Error = "Please select a CSV file";
                return View("Index");
            }
            else
            {
               if(postedFile.FileName.EndsWith("csv"))
               {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    // Read the contents of CSV file:
                    sales = csvImport.ParseCSV(filePath);

                    // Bulk Insert the sales record
                    csvImport.BulkInsertSalesRecords(sales);

                    return View("Index", sales.Take(100));
               }
               else
               {
                    ViewBag.Error = "File type is incorrect <br>";
                    return View("Index");
                }
            }
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sales/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sales/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
