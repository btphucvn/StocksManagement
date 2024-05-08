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
    internal class RowController
    {
        public static Row GetRowBy_Code_Name(string code,string name)
        {
            DBConnect connect = new DBConnect();
            MySqlConnection conn = connect.ConnectMySQL();
            string query = "";
            query = "SELECT * FROM Stock where code = '" + code+"' AND Name='"+name+"'";
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                command.Connection = conn;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Row row = new Row();
                    row.ID = reader.GetInt32(reader.GetOrdinal("id"));
                    row.Code = reader.GetString(reader.GetOrdinal("Code"));
                    row.Name = reader.GetString(reader.GetOrdinal("Name"));
                    row.Type = reader.GetInt32(reader.GetOrdinal("Type"));
                    row.Sort = reader.GetInt32(reader.GetOrdinal("Sort"));

                    return row;

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

        public static List<Row> GetRowByCode(string code)
        {
            DBConnect connect = new DBConnect();
            MySqlConnection conn = connect.ConnectMySQL();
            string query = "";
            query = "SELECT * FROM Row where code = '" + code + "'";
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                command.Connection = conn;
                MySqlDataReader reader = command.ExecuteReader();
                List<Row> rows = new List<Row>();   
                while (reader.Read())
                {
                    Row row = new Row();
                    row.ID = reader.GetInt32(reader.GetOrdinal("id"));
                    row.Code = reader.GetString(reader.GetOrdinal("Code"));
                    row.Name = reader.GetString(reader.GetOrdinal("Name"));
                    row.Type = reader.GetInt32(reader.GetOrdinal("Type"));
                    row.Sort = reader.GetInt32(reader.GetOrdinal("Sort"));
                    rows.Add(row);

                }
                conn.Close();
                return rows;
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

        public static string Insert(Row row)
        {
            DBConnect dBConnect = new DBConnect();
            MySqlConnection conn = dBConnect.ConnectMySQL();

            string query = "insert into row(ID,Name,Code,Type,Sort) values('"
                + row.ID + "','"
                + row.Name + "','"
                + row.Code + "','"
                + row.Type + "','"
                + row.Sort + "');";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            try
            {
                conn.Open();
                cmd.ExecuteReader();
                int id = (int)cmd.LastInsertedId;

                conn.Close();
                return id.ToString();
            }
            catch (Exception a)
            {
                return a.Message + " Code: " + row.Code + " Name: " + row.Name;
            }
            finally
            {
                conn.Close();
            }

        }
    }
}
