using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksManagement.DB
{
    public class DBConnect
    {
        public MySqlConnection ConnectMySQL()
        {

            string host = "localhost";
            int port = 3306;
            string database = "stocks";
            string username = "root";
            string password = "";


            String connString = "Server=" + host + ";Database=" + database
                + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }

    }
}
