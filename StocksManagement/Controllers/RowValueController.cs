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
    public static class RowValueController
    {
        public static List<RowValue> GetRowValueByRowID(int id)
        {
            DBConnect connect = new DBConnect();
            MySqlConnection conn = connect.ConnectMySQL();
            string query = "";
            query = "SELECT * FROM RowValue WHERE RowID = " + id + " Order By TimeStamp";
            List<RowValue> list = new List<RowValue>();
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                command.Connection = conn;
                MySqlDataReader reader = command.ExecuteReader();
                if (id == 280)
                {
                    int sa = 0;
                }
                while (reader.Read())
                {
                    RowValue row = new RowValue();
                    row.ID = reader.GetInt32(reader.GetOrdinal("id"));
                    try
                    {
                        row.Value = reader.GetDouble(reader.GetOrdinal("Value"));

                    }
                    catch (Exception ex) { }
                    row.TimeStamp = reader.GetDouble(reader.GetOrdinal("TimeStamp"));
                    row.RowID = reader.GetInt32(reader.GetOrdinal("RowID"));

                    list.Add(row);

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
        public static RowValue GetRowValueByRowIDAndTimeStamp(double rowID, double timeStamp)
        {
            DBConnect connect = new DBConnect();
            MySqlConnection conn = connect.ConnectMySQL();
            string query = "";
            query = "SELECT * FROM RowValue WHERE RowID = " + rowID + " AND TimeStamp=" + timeStamp;

            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = query;
                command.Connection = conn;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RowValue row = new RowValue();
                    row.ID = reader.GetInt32(reader.GetOrdinal("id"));
                    try
                    {
                        row.Value = reader.GetDouble(reader.GetOrdinal("Value"));

                    }
                    catch (Exception ex) { }
                    row.TimeStamp = reader.GetDouble(reader.GetOrdinal("TimeStamp"));
                    row.RowID = reader.GetInt32(reader.GetOrdinal("RowID"));
                    row.Quater = reader.GetString(reader.GetOrdinal("Quater"));
                    conn.Close();
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

        public static string InsertAndUpdate(RowValue rowvalue)
        {
            RowValue tmp = GetRowValueByRowIDAndTimeStamp(rowvalue.RowID, rowvalue.TimeStamp);
            if (tmp != null)
            {

                try
                {
                    rowvalue.ID = tmp.ID;
                    string err = Update(rowvalue);
                    if (err != "")
                    {
                        return err;
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            else
            {
                rowvalue.ID = 0;
                string tmpID = Insert(rowvalue);
                double tmpNumber = 0;
                if (!Double.TryParse(tmpID, out tmpNumber))
                {
                    return "Lỗi Insert RowValue: " + tmpID;
                }
                //_context.Rowvalues.Add(rowvalue);

            }
            return "";
        }
        public static string Insert(RowValue rowValue)
        {
            DBConnect dBConnect = new DBConnect();
            MySqlConnection conn = dBConnect.ConnectMySQL();
            string query = "insert into rowvalue(RowID,Value,TimeStamp,Quater) values('"
                + rowValue.RowID + "','"
                + rowValue.Value + "','"
                + rowValue.TimeStamp + "','"
                + rowValue.Quater + "');";
            if (rowValue.Value == null)
            {
                query = "insert into rowvalue(RowID,TimeStamp,Quater) values('"
                    + rowValue.RowID + "','"
                    + rowValue.TimeStamp + "','"
                    + rowValue.Quater + "');";
            }




            MySqlCommand cmd = new MySqlCommand(query, conn);
            try
            {
                conn.Open();
                cmd.ExecuteReader();
                int id = (int)cmd.LastInsertedId;
                conn.Close();
                return id.ToString();
                //return Task.Delay(10000)
                //        .ContinueWith(t => id.ToString());
            }
            catch (Exception a)
            {
                return a.Message;
            }
            finally
            {
                conn.Close();
            }
            return "";
            //return Task.Delay(10000)
            //                       .ContinueWith(t => "");
        }

        public static string Update(RowValue rowvalue)
        {
            DBConnect dBConnect = new DBConnect();
            MySqlConnection conn = dBConnect.ConnectMySQL();
            string query = "UPDATE Rowvalue "
                + "SET value = '" + rowvalue.Value + "' "
                + "WHERE id = '" + rowvalue.ID + "'";
            if (rowvalue.Value == null)
            {
                query = "UPDATE Rowvalue "
                + "SET value = NULL "
                + "WHERE id = '" + rowvalue.ID + "'";
            }
            MySqlCommand cmd = new MySqlCommand(query, conn);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception a)
            {
                return a.Message;
            }
            finally
            {
                conn.Close();
            }

            return "";

        }
    }
}
