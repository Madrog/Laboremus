using OfflineStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Configuration;

namespace OfflineStore.BL
{
    public class CsvImport
    {
        

        public List<SalesRecord> ParseCSV(string csvFilePath)
        {
            return File.ReadAllLines(csvFilePath)
                .AsParallel()
                .Skip(1)
                .Where(row => row.Length > 0)
                .Select(ParseNow).ToList();
        }

        private SalesRecord ParseNow(string row)
        {
            var columns = row.Split(',');

            return new SalesRecord()
            {
                Region = columns[0].Trim(),
                Country = columns[1].Trim(),
                ItemType = columns[2].Trim(),
                SalesChannel = columns[3].Trim(),
                OrderPriority = columns[4][0],
                OrderDate = DateConversion(columns[5].Trim()),
                OrderID = long.Parse(columns[6]),
                ShipDate = DateConversion(columns[7].Trim()),
                UnitsSold = long.Parse(columns[8]),
                UnitPrice = Decimal.Parse(columns[9]),
                UnitCost = Decimal.Parse(columns[10]),
                TotalRevenue = Decimal.Parse(columns[11]),
                TotalCost = Decimal.Parse(columns[12]),
                TotalProfit = Decimal.Parse(columns[13])
            };
        }

        private DateTime DateConversion(string dateString)
        {
            DateTime convertedDate = new DateTime();
            int year, month, day;

            year = 0000;
            month = 0;
            day = 0;

            if (!DateTime.TryParse(dateString, out convertedDate))
            {
                var values = dateString.Split('/');

                day = Int32.Parse(values[1]);
                month = Int32.Parse(values[0]);
                year = Int32.Parse(values[2]);

                convertedDate = new DateTime(year, month, day);
            }

            return convertedDate;
        }

        public void BulkInsertSalesRecords(IEnumerable<SalesRecord> salesList)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SalesConnection"].ConnectionString;

            // create datatable
            var dt = new DataTable();
            dt.Columns.Add("region", typeof(string));
            dt.Columns.Add("country", typeof(string));
            dt.Columns.Add("item_type", typeof(string));
            dt.Columns.Add("sales_channel", typeof(string));
            dt.Columns.Add("order_priority", typeof(string));
            dt.Columns.Add("order_date", typeof(DateTime));
            dt.Columns.Add("order_id", typeof(long));
            dt.Columns.Add("ship_date", typeof(DateTime));
            dt.Columns.Add("units_sold", typeof(long));
            dt.Columns.Add("unit_price", typeof(Decimal));
            dt.Columns.Add("unit_cost", typeof(Decimal));
            dt.Columns.Add("total_revenue", typeof(Decimal));
            dt.Columns.Add("total_cost", typeof(Decimal));
            dt.Columns.Add("total_profit", typeof(Decimal));

            foreach (var salesItem in salesList)
            {
                dt.Rows.Add(new object[] {
                    salesItem.Region,
                    salesItem.Country,
                    salesItem.ItemType,
                    salesItem.SalesChannel,
                    salesItem.OrderPriority,
                    salesItem.OrderDate,
                    salesItem.OrderID,
                    salesItem.ShipDate,
                    salesItem.UnitsSold,
                    salesItem.UnitPrice,
                    salesItem.UnitCost,
                    salesItem.TotalRevenue,
                    salesItem.TotalCost,
                    salesItem.TotalProfit
                });
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, transaction))
                    {
                        try
                        {
                            bulkCopy.DestinationTableName = "Sales";
                            bulkCopy.WriteToServer(dt);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            con.Close();
                        }
                    }
                }

            }
        }
    }
}