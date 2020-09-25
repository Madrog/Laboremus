using OfflineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace OfflineStore.BL
{
    public class SalesBL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SalesConnection"].ConnectionString;

        public List<SalesRecord> GetSalesRecords()
        {
            List<SalesRecord> sales = new List<SalesRecord>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetTop1000SalesRecords", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SalesRecord saleItem = new SalesRecord
                    {
                        Region = rdr["region"].ToString(),
                        Country = rdr["country"].ToString(),
                        ItemType = rdr["item_type"].ToString(),
                        SalesChannel = rdr[3].ToString(),
                        OrderPriority = rdr[4].ToString()[0],
                        OrderDate = Convert.ToDateTime(rdr[5].ToString()),
                        OrderID = long.Parse(rdr[6].ToString()),
                        ShipDate = Convert.ToDateTime(rdr[7].ToString()),
                        UnitsSold = long.Parse(rdr[8].ToString()),
                        UnitPrice = Decimal.Parse(rdr[9].ToString()),
                        UnitCost = Decimal.Parse(rdr[10].ToString()),
                        TotalRevenue = Decimal.Parse(rdr[11].ToString()),
                        TotalCost = Decimal.Parse(rdr[12].ToString()),
                        TotalProfit = Decimal.Parse(rdr[13].ToString())
                    };

                    sales.Add(saleItem);
                }
            }

            return sales;
        }

        public Dictionary<string, decimal> GetTop5ProfitableItemTypes(DateTime date1, DateTime date2)
        {
            Dictionary<string, decimal> top5ItemTypes = new Dictionary<string, decimal>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spTop5ProfitableItemTypes" +
                    "", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter paramDate1 = new SqlParameter();
                paramDate1.ParameterName = "@DATE1";
                paramDate1.Value = date1;
                cmd.Parameters.Add(paramDate1);

                SqlParameter paramDate2 = new SqlParameter();
                paramDate2.ParameterName = "@DATE2";
                paramDate2.Value = date2;
                cmd.Parameters.Add(paramDate2);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if(!string.IsNullOrWhiteSpace(rdr[0].ToString()))
                    {
                        top5ItemTypes.Add(
                        rdr[0].ToString(),
                        Convert.ToDecimal(rdr[1].ToString())
                        );
                    }
                    
                }
            }

            return top5ItemTypes;
        }

        public decimal GetTotalProfitMade(DateTime date1, DateTime date2)
        {
            decimal totalProfit = 0.0M;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spTotalProfitMade" +
                    "", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter paramDate1 = new SqlParameter();
                paramDate1.ParameterName = "@DATE1";
                paramDate1.Value = date1;
                cmd.Parameters.Add(paramDate1);

                SqlParameter paramDate2 = new SqlParameter();
                paramDate2.ParameterName = "@DATE2";
                paramDate2.Value = date2;
                cmd.Parameters.Add(paramDate2);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if(!string.IsNullOrWhiteSpace(rdr[0].ToString()))
                        totalProfit = Convert.ToDecimal(rdr[0].ToString());          
                }
            }

            return totalProfit;
        }
    }
}