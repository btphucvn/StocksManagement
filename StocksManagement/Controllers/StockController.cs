using MySql.Data.MySqlClient;
using StocksManagement.DB;
using StocksManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksManagement.Controllers
{
    public static class StockController
    {
        public static List<Stock> GetAllStock(int from=0, int to = 0)
        {
            DBConnect connect = new DBConnect();
            MySqlConnection conn = connect.ConnectMySQL();
            string query = "";
            query = "SELECT * FROM Stock";
            if(from > 0 && to >0)
            {
                query = query + " WHERE" + " Sort>=" + from + " AND Sort<=" + to;
            }
            List<Stock> list = new List<Stock>();
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                command.Connection = conn;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Stock stock = new Stock();
                    stock.Code = reader.GetString(reader.GetOrdinal("Code"));
                    stock.Name = reader.GetString(reader.GetOrdinal("Name"));
                    stock.Exchange = reader.GetString(reader.GetOrdinal("Exchange"));
                    stock.IndustryID = reader.GetInt32(reader.GetOrdinal("IndustryID"));
                    stock.Rows = RowController.GetRowByCode(stock.Code);
                    list.Add(stock);

                }
                conn.Close();

            }

            catch (Exception e)
            {
                conn.Close();
            }
            finally
            {
                conn.Close();
            }
            return list;
        }
        public static Stock GetStockByCode(string code)
        {
            DBConnect connect = new DBConnect();
            MySqlConnection conn = connect.ConnectMySQL();
            string query = "";
            query = "SELECT * FROM Stock where code = '"+code+"'";
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                command.Connection = conn;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Stock stock = new Stock();
                    stock.Code = reader.GetString(reader.GetOrdinal("Code"));
                    stock.Name = reader.GetString(reader.GetOrdinal("Name"));
                    stock.Exchange = reader.GetString(reader.GetOrdinal("Exchange"));

                    return stock;

                }
                conn.Close();

            }

            catch (Exception e)
            {
                conn.Close();
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        public static int Insert(Stock stock)
        {
            DBConnect dBConnect = new DBConnect();
            MySqlConnection conn = dBConnect.ConnectMySQL();
            if (GetStockByCode(stock.Code) != null)
            {
                return stock.ID;
            }
            string query = "insert into stock(Code,Name,Exchange) values('"
                + stock.Code + "','"
                + stock.Name + "','"
                + stock.Exchange + "');";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            try
            {
                conn.Open();
                cmd.ExecuteReader();
                int id = (int)cmd.LastInsertedId;
                
                conn.Close();
                return id;
            }
            catch (Exception a)
            {
                return -1;
            }
            finally
            {
                conn.Close();
            }

        }
    }
}
